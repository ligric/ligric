using System.Collections.ObjectModel;
using Utils;
using Ligric.Protobuf;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using static Ligric.Protobuf.Futures;
using Ligric.Business.Interfaces;
using System.Collections;

namespace Ligric.Business.Clients.Futures
{
	public class ValuesService : IValuesService, ISession
	{
		private int syncValuesChanged = 0;
		private readonly Dictionary<string, decimal> _values = new Dictionary<string, decimal>();
		private readonly Dictionary<long, CancellationTokenSource> attachedTradesCalcellationTokens = new Dictionary<long, CancellationTokenSource>();

		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal ValuesService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<string, decimal> Values => new ReadOnlyDictionary<string, decimal>(_values);

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (attachedTradesCalcellationTokens.TryGetValue(userApiId, out CancellationTokenSource cts)
			&& cts != null && !cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			var newTradesCancelationTokenSource = new CancellationTokenSource();
			attachedTradesCalcellationTokens.Add(userApiId, newTradesCancelationTokenSource);
			return StreamValuesSubscribeCall(userId, userApiId, newTradesCancelationTokenSource.Token);
		}

		public void DetachStream(long userApiId)
		{
			if (attachedTradesCalcellationTokens.TryGetValue(userApiId, out CancellationTokenSource cts))
			{
				cts?.Cancel();
				cts?.Dispose();
				attachedTradesCalcellationTokens.Remove(userApiId);
			}
			_values.ClearAndRiseEvent(this, ValuesChanged, ref syncValuesChanged);
		}

		#region Session
		public void InitializeSession()
		{

		}

		public void ClearSession()
		{
			foreach (var item in attachedTradesCalcellationTokens)
			{
				item.Value?.Cancel();
				item.Value?.Dispose();
			}
			attachedTradesCalcellationTokens.Clear();
			_values.ClearAndRiseEvent(this, ValuesChanged, ref syncValuesChanged);
			syncValuesChanged = 0;
		}

		public void Dispose()
		{
			foreach (var item in attachedTradesCalcellationTokens)
			{
				item.Value?.Cancel();
				item.Value?.Dispose();
			}
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
			lock (((ICollection)_values).SyncRoot)
			{
				var symbol = valuesChanged.Value.Symbol;
				var value = decimal.Parse(valuesChanged.Value.Value);
				switch (valuesChanged.Action)
				{
					case Protobuf.Action.Added:
						_values.SetAndRiseEvent(this, ValuesChanged, symbol, value, ref syncValuesChanged);
						break;
					case Protobuf.Action.Removed:
						_values.RemoveAndRiseEvent(this, ValuesChanged, symbol, ref syncValuesChanged);
						break;
					case Protobuf.Action.Changed: goto case Protobuf.Action.Added;
				}
			}
		}
	}
}
