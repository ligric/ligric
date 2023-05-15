using System.Collections.ObjectModel;
using Utils;
using Ligric.Business.Authorization;
using Ligric.Protobuf;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using static Ligric.Protobuf.Futures;
using Ligric.Business.Interfaces;

namespace Ligric.Business.Clients.Futures
{
	public class ValuesService : IValuesService, ISession
	{
		private int syncValuesChanged = 0;
		private readonly Dictionary<string, decimal> _values = new Dictionary<string, decimal>();
		private CancellationTokenSource? _valuesSubscribeCalcellationToken;
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
			if (_valuesSubscribeCalcellationToken != null
				&& !_valuesSubscribeCalcellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			_valuesSubscribeCalcellationToken = new CancellationTokenSource();
			return StreamValuesSubscribeCall(userId, userApiId, _valuesSubscribeCalcellationToken.Token);
		}

		public void DetachStream()
		{
			_valuesSubscribeCalcellationToken?.Cancel();
		}

		#region Session
		public void InitializeSession()
		{

		}

		public void ClearSession()
		{
			DetachStream();
			syncValuesChanged = 0;
			_values.Clear();
		}

		public void Dispose()
		{
			DetachStream();
			_valuesSubscribeCalcellationToken?.Dispose();
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
