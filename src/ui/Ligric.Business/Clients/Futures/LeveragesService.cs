using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Google.Protobuf.WellKnownTypes;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;
using Utils;
using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients.Futures
{
	public class LeveragesService : ILeveragesService, ISession
	{
		private readonly List<ExchangedEntity<LeverageDto>> _leverages = new List<ExchangedEntity<LeverageDto>>();
		private readonly Dictionary<long, CancellationTokenSource> attachedLeveragesCalcellationTokens = new Dictionary<long, CancellationTokenSource>();

		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal LeveragesService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyCollection<ExchangedEntity<LeverageDto>> Leverages => new ReadOnlyCollection<ExchangedEntity<LeverageDto>>(_leverages);

		public event NotifyCollectionChangedEventHandler? LeveragesChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (attachedLeveragesCalcellationTokens.TryGetValue(userApiId, out CancellationTokenSource cts)
			&& cts != null && !cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");
			var newLeveragesCancelationTokenSource = new CancellationTokenSource();
			attachedLeveragesCalcellationTokens.Add(userApiId, newLeveragesCancelationTokenSource);
			return StreamApiSubscribeCall(userId, userApiId, newLeveragesCancelationTokenSource.Token);
		}

		public void DetachStream(long userApiId)
		{
			if (attachedLeveragesCalcellationTokens.TryGetValue(userApiId, out CancellationTokenSource cts))
			{
				cts?.Cancel();
				cts?.Dispose();
				attachedLeveragesCalcellationTokens.Remove(userApiId);
			}
			_leverages.ResetAndRiseEvent(this, LeveragesChanged);
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			foreach (var item in attachedLeveragesCalcellationTokens)
			{
				item.Value?.Cancel();
				item.Value?.Dispose();
			}
			attachedLeveragesCalcellationTokens.Clear();
			_leverages.ResetAndRiseEvent(this, LeveragesChanged);
		}

		public void Dispose()
		{
			foreach (var item in attachedLeveragesCalcellationTokens)
			{
				item.Value?.Cancel();
				item.Value?.Dispose();
			}
		}
		#endregion

		private Task StreamApiSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.LeverageSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(call.Dispose)
				.ForEachAsync(OnLeveragesChanged, token);
		}

		private void OnLeveragesChanged(LeverageChanged changes)
		{
			lock (((ICollection)_leverages).SyncRoot)
			{
				switch (changes.Action)
				{
					case Protobuf.Action.Added:
						var leverageDto = changes.Leverage.ToFuturesLeverageDto();
						_leverages.AddAndRiseEvent(this, LeveragesChanged, new ExchangedEntity<LeverageDto>(Guid.Parse(changes.ExchangeId), leverageDto));
						break;
					case Protobuf.Action.Changed:
						goto case Protobuf.Action.Added;
				}
			}
		}
	}
}
