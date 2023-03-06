using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Ligric.Domain.Types.Future;
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
	public class PositionsService : IPositionsService
	{
		private int syncOrderChanged = 0;
		private readonly Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();
		private CancellationTokenSource? _futuresSubscribeCalcellationToken;
		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal PositionsService(
			GrpcChannel channel,
			IMetadataManager metadataRepos,
			IAuthorizationService authorizationService)
		{
			_metadataManager = metadataRepos;
			_authorizationService = authorizationService;
			_futuresClient = new FuturesClient(channel);
		}

		public IReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_futuresSubscribeCalcellationToken != null
				&& !_futuresSubscribeCalcellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _authorizationService.CurrentUser.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			_futuresSubscribeCalcellationToken = new CancellationTokenSource();
			return StreamApiSubscribeCall(userId, userApiId, _futuresSubscribeCalcellationToken.Token);
		}

		public void DetachStream() => throw new NotImplementedException();
		public void Dispose()
		{

		}

		private Task StreamApiSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.PositionsSubscribe(
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

		private void OnFuturesChanged(PositionsChanged positionsChanged)
		{
			switch (positionsChanged.Action)
			{
				case Protos.Action.Added:
					var positionDto = positionsChanged.Position.ToFuturesPositionDto();
					_positions.SetAndRiseEvent(this, PositionsChanged, positionsChanged.Position.Id, positionDto, ref syncOrderChanged);
					break;
				case Protos.Action.Removed:
					_positions.RemoveAndRiseEvent(this, PositionsChanged, positionsChanged.Position.Id, ref syncOrderChanged);
					break;
				case Protos.Action.Changed: goto case Protos.Action.Added;
			}
		}
	}
}
