using System.Collections.ObjectModel;
using Utils;
using Ligric.Protobuf;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using static Ligric.Protobuf.Futures;
using Ligric.Business.Interfaces;
using System.Collections;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesTradesService : IFuturesTradesService, ISession
	{
		private int syncValuesChanged = 0;
		private CancellationTokenSource? _cts;
		private readonly Dictionary<string, decimal> _trades = new Dictionary<string, decimal>();

		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal FuturesTradesService(
			FuturesClient futuresClient,
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

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			var cts = new CancellationTokenSource();
			_cts = cts;
			return StreamValuesSubscribeCall(userId, userApiId, cts.Token);
		}

		public void DetachStream()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_trades.ClearAndRiseEvent(this, TradesChanged, ref syncValuesChanged);
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_trades.ClearAndRiseEvent(this, TradesChanged, ref syncValuesChanged);
			syncValuesChanged = 0;
		}

		public void Dispose()
		{
			_cts?.Cancel();
			_cts?.Dispose();
		}
		#endregion

		private Task StreamValuesSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.ValuesSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(call.Dispose)
				.ForEachAsync(OnFuturesChanged, token);
		}

		private void OnFuturesChanged(ValuesChanged valuesChanged)
		{
			lock (((ICollection)_trades).SyncRoot)
			{
				var symbol = valuesChanged.Value.Symbol;
				var value = decimal.Parse(valuesChanged.Value.Value);
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
	}
}
