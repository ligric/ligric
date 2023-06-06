using System.Collections.ObjectModel;
using Utils;
using Ligric.Protobuf;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using static Ligric.Protobuf.BinanceFuturesTrades;
using Ligric.Business.Interfaces;
using System.Collections;
using Ligric.Core.Types.Future;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesTradesService : IFuturesTradesService, ISession
	{
		private int syncValuesChanged = 0;
		private CancellationTokenSource? _cts;
		private readonly Dictionary<string, decimal> _trades = new Dictionary<string, decimal>();

		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly BinanceFuturesTradesClient _futuresClient;

		internal FuturesTradesService(
			BinanceFuturesTradesClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<string, decimal> Trades => new ReadOnlyDictionary<string, decimal>(_trades);

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? TradesChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_cts != null && !_cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStream] UserId is null");

			var cts = new CancellationTokenSource();
			_cts = cts;
			return StreamValuesSubscribeCall(userId, userApiId, cts.Token);
		}

		public void DetachStream()
		{
			StopStream();
			_trades.ClearAndRiseEvent(this, TradesChanged, new Dictionary<string, decimal>(_trades), ref syncValuesChanged);
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			StopStream();
			_trades.ClearAndRiseEvent(this, TradesChanged, new Dictionary<string, decimal>(_trades), ref syncValuesChanged);
			syncValuesChanged = 0;
		}

		public void Dispose()
		{
			StopStream();
		}
		#endregion

		private Task StreamValuesSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.TradesSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(call.Dispose)
				.ForEachAsync(OnFuturesChanged, token);
		}

		private void OnFuturesChanged(TradesChanged valuesChanged)
		{
			lock (((ICollection)_trades).SyncRoot)
			{
				var symbol = valuesChanged.Trade.Symbol;
				var value = decimal.Parse(valuesChanged.Trade.Value);
				switch (valuesChanged.Action)
				{
					case Protobuf.Action.Added:
						_trades.SetAndRiseEvent(this, TradesChanged, symbol, value, ref syncValuesChanged);
						break;
					case Protobuf.Action.Removed:
						_trades.RemoveAndRiseEvent(this, TradesChanged, symbol, ref syncValuesChanged);
						break;
					case Protobuf.Action.Changed: goto case Protobuf.Action.Added;
				}
			}
		}

		private void StopStream()
		{
			if (_cts != null)
			{
				_cts.Cancel();
				_cts.Dispose();
				_cts = null;
			}
		}
	}
}
