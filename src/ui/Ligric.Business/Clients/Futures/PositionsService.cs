using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;
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
		private int syncPositionsChanged = 0;
		private readonly Dictionary<long, ExchangedEntity<FuturesPositionDto>> _positions = new Dictionary<long, ExchangedEntity<FuturesPositionDto>>();
		private readonly Dictionary<long, CancellationTokenSource> attachedPositionsCalcellationTokens = new Dictionary<long, CancellationTokenSource>();
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
			if (attachedPositionsCalcellationTokens.TryGetValue(userApiId, out CancellationTokenSource cts)
				&& cts != null && !cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			var newPositionCancelationTokenSource = new CancellationTokenSource();
			attachedPositionsCalcellationTokens.Add(userApiId, newPositionCancelationTokenSource);
			return StreamApiSubscribeCall(userId, userApiId, newPositionCancelationTokenSource.Token);
		}

		public void DetachStream(long userApiId)
		{
			if (attachedPositionsCalcellationTokens.TryGetValue(userApiId, out CancellationTokenSource cts))
			{
				cts?.Cancel();
				cts?.Dispose();
				attachedPositionsCalcellationTokens.Remove(userApiId);
			}
		}

		#region Session
		public void InitializeSession()
		{

		}

		public void ClearSession()
		{
			foreach (var item in attachedPositionsCalcellationTokens)
			{
				item.Value?.Cancel();
				item.Value?.Dispose();
			}
			attachedPositionsCalcellationTokens.Clear();
			_positions.ClearAndRiseEvent(this, PositionsChanged, ref syncPositionsChanged);
			syncPositionsChanged = 0;
		}

		public void Dispose()
		{
			foreach (var item in attachedPositionsCalcellationTokens)
			{
				item.Value?.Cancel();
				item.Value?.Dispose();
			}
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
