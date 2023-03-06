using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Grpc.Net.Client;
using static Ligric.Protos.Futures;
using Utils;
using Ligric.Business.Authorization;
using System.Threading;
using Ligric.Protos;
using System.Linq;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;

namespace Ligric.Business.Clients.Futures
{
	public class ValuesService : IValuesService
	{
		private int syncValuesChanged = 0;
		private readonly Dictionary<string, decimal> _values = new Dictionary<string, decimal>();
		private CancellationTokenSource? _valuesSubscribeCalcellationToken;
		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal ValuesService(
			GrpcChannel channel,
			IMetadataManager metadataRepos,
			IAuthorizationService authorizationService)
		{
			_metadataManager = metadataRepos;
			_authorizationService = authorizationService;

			_futuresClient = new FuturesClient(channel);
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

			var userId = _authorizationService.CurrentUser.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			_valuesSubscribeCalcellationToken = new CancellationTokenSource();
			return StreamValuesSubscribeCall(userId, userApiId, _valuesSubscribeCalcellationToken.Token);
		}

		public void DetachStream() => throw new NotImplementedException();

		public void Dispose()
		{

		}

		private Task StreamValuesSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.ValuesSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(() => call.Dispose())
				.ForEachAsync((api) =>
				{
					OnFuturesChanged(api);
				}, token);
		}

		private void OnFuturesChanged(ValuesChanged valuesChanged)
		{
			var symbol = valuesChanged.Value.Symbol;
			var value = decimal.Parse(valuesChanged.Value.Value);
			switch (valuesChanged.Action)
			{
				case Protos.Action.Added:
					_values.SetAndRiseEvent(this, ValuesChanged, symbol, value, ref syncValuesChanged);
					break;
				case Protos.Action.Removed:
					_values.RemoveAndRiseEvent(this, ValuesChanged, symbol, ref syncValuesChanged);
					break;
				case Protos.Action.Changed: goto case Protos.Action.Added;
			}
		}
	}
}
