using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;
using Utils;
using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesLeveragesService : IFuturesLeveragesService, ISession
	{
		private CancellationTokenSource? _cts;
		private readonly List<LeverageDto> _leverages = new List<LeverageDto>();

		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal FuturesLeveragesService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyCollection<LeverageDto> Leverages => new ReadOnlyCollection<LeverageDto>(_leverages);

		public event NotifyCollectionChangedEventHandler? LeveragesChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_cts != null && !_cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");
			var cts = new CancellationTokenSource();
			_cts = cts;
			return StreamApiSubscribeCall(userId, userApiId, cts.Token);
		}

		public void DetachStream()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_leverages.ResetAndRiseEvent(this, LeveragesChanged);
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_leverages.ResetAndRiseEvent(this, LeveragesChanged);
		}

		public void Dispose()
		{
			_cts?.Cancel();
			_cts?.Dispose();
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
						_leverages.AddAndRiseEvent(this, LeveragesChanged, leverageDto);
						break;
					case Protobuf.Action.Changed:
						goto case Protobuf.Action.Added;
				}
			}
		}
	}
}
