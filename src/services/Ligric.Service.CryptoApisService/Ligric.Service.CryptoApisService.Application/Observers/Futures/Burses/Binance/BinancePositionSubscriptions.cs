using Ligric.Core.Types.Future;
using System.Reactive.Linq;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Utils;
using Ligric.Service.CryptoApisService.Domain.Extensions;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinancePositionSubscriptions : IPositionSubscriptions
	{
		private readonly IApiRepository _apiRepository;
		private readonly BinanceFuturesApiSubscriptions _futuresApiSubscriptions;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>? positionsChanged;

		public BinancePositionSubscriptions(
			BinanceFuturesApiSubscriptions futuresApiSubscriptions,
			IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
			_futuresApiSubscriptions = futuresApiSubscriptions;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable()
		{
			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>((x)
				=> positionsChanged += x, (x) => positionsChanged -= x);

			return updatedApiStateNotifications;
		}

		public void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();

			_futuresApiSubscriptions.AttachSubscriptionIdToApi(api, userId, out subscribedStreamId, out var burseSession);
			burseSession.PositionsChanged += OnPositionsChanged;
		}

		public void UnsubscribeStream(Guid subscribedStreamId)
		{
			_futuresApiSubscriptions.DetachSubscriptionAndTryToRemoveApiSubscriptionObject(subscribedStreamId);
		}

		private void OnPositionsChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs) obj)
			=> positionsChanged?.Invoke(obj);

	}
}
