using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using Ligric.Protobuf;
using static Ligric.Protobuf.Futures;
using Ligric.Business.Interfaces;
using System.Collections;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesPositionsService : IFuturesPositionsService, ISession
	{
		private int syncPositionsChanged = 0;
		private CancellationTokenSource? _cts;

		private readonly Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();
		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal FuturesPositionsService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_cts != null && !_cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStream] UserId is null");

			var cts = new CancellationTokenSource();
			_cts = cts;
			return StreamApiSubscribeCall(userId, userApiId, cts.Token);
		}

		public void DetachStream()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_positions.ClearAndRiseEvent(this, PositionsChanged, ref syncPositionsChanged);
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_positions.ClearAndRiseEvent(this, PositionsChanged, ref syncPositionsChanged);
			syncPositionsChanged = 0;
		}

		public void Dispose()
		{
			_cts?.Cancel();
			_cts?.Dispose();
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
			lock (((ICollection)_positions).SyncRoot)
			{
				switch (positionsChanged.Action)
				{
					case Protobuf.Action.Added:
						var exchangedPositionDto = positionsChanged.Position.ToFuturesPositionDto();
						_positions.SetAndRiseEvent(this, PositionsChanged, positionsChanged.Position.Id, exchangedPositionDto, ref syncPositionsChanged);
						break;
					case Protobuf.Action.Removed:
						_positions.RemoveAndRiseEvent(this, PositionsChanged, positionsChanged.Position.Id, ref syncPositionsChanged);
						break;
					case Protobuf.Action.Changed: goto case Protobuf.Action.Added;
				}
			}
		}
	}
}
