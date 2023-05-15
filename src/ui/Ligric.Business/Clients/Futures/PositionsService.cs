using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;
using Ligric.Business.Authorization;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using Ligric.Protobuf;
using static Ligric.Protobuf.Futures;
using Ligric.Core.Types;
using Ligric.Business.Interfaces;

namespace Ligric.Business.Clients.Futures
{
	public class PositionsService : IPositionsService, ISession
	{
		private int syncOrderChanged = 0;
		private readonly Dictionary<long, ExchangedEntity<FuturesPositionDto>> _positions = new Dictionary<long, ExchangedEntity<FuturesPositionDto>>();
		private CancellationTokenSource? _futuresSubscribeCalcellationToken;
		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal PositionsService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<long, ExchangedEntity<FuturesPositionDto>> Positions => new ReadOnlyDictionary<long, ExchangedEntity<FuturesPositionDto>>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesPositionDto>>>? PositionsChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_futuresSubscribeCalcellationToken != null
				&& !_futuresSubscribeCalcellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			_futuresSubscribeCalcellationToken = new CancellationTokenSource();
			return StreamApiSubscribeCall(userId, userApiId, _futuresSubscribeCalcellationToken.Token);
		}

		public void DetachStream()
		{
			_futuresSubscribeCalcellationToken?.Cancel();
		}

		#region Session
		public void InitializeSession()
		{

		}

		public void ClearSession()
		{
			DetachStream();
			_positions.Clear();
			syncOrderChanged = 0;
		}

		public void Dispose()
		{
			DetachStream();
			_futuresSubscribeCalcellationToken?.Dispose();
		}
		#endregion

		private Task StreamApiSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.PositionsSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(call.Dispose)
				.ForEachAsync(OnFuturesChanged, token);
		}

		private void OnFuturesChanged(PositionsChanged positionsChanged)
		{
			switch (positionsChanged.Action)
			{
				case Protobuf.Action.Added:
					var exchangedPositionDto = new ExchangedEntity<FuturesPositionDto>(
						Guid.Parse(positionsChanged.ExchangeId),
						positionsChanged.Position.ToFuturesPositionDto());

					_positions.SetAndRiseEvent(this, PositionsChanged, positionsChanged.Position.Id, exchangedPositionDto, ref syncOrderChanged);
					break;
				case Protobuf.Action.Removed:
					_positions.RemoveAndRiseEvent(this, PositionsChanged, positionsChanged.Position.Id, ref syncOrderChanged);
					break;
				case Protobuf.Action.Changed: goto case Protobuf.Action.Added;
			}
		}
	}
}
