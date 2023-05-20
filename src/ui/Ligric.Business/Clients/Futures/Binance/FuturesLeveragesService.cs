using System.Collections;
using System.Collections.ObjectModel;
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
		private int sync = 0;
		private CancellationTokenSource? _cts;
		private readonly Dictionary<string, LeverageDto> _leverages = new Dictionary<string, LeverageDto>();

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

		public IReadOnlyDictionary<string, LeverageDto> Leverages => new ReadOnlyDictionary<string, LeverageDto>(_leverages);

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, LeverageDto>>? LeveragesChanged;

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
			_leverages.ClearAndRiseEvent(this, LeveragesChanged, ref sync);
			sync = 0;
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_leverages.ClearAndRiseEvent(this, LeveragesChanged, ref sync);
			sync = 0;
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
						var leverageDto = Extensions.TypeExtensions.ToFuturesLeverageDto(changes.Leverage);
						_leverages.SetAndRiseEvent(this, LeveragesChanged, leverageDto.Symbol, leverageDto, ref sync);
						break;
					case Protobuf.Action.Changed:
						goto case Protobuf.Action.Added;
				}
			}
		}
	}
}
