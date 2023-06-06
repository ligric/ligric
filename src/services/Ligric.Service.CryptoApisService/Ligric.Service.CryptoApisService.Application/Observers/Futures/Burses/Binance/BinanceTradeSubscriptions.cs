using System.Reactive.Linq;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Utils;
using Ligric.Service.CryptoApisService.Domain.Extensions;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinanceTradeSubscriptions : ITradeSubscriptions
	{
		private readonly IApiRepository _apiRepository;
		private readonly BinanceFuturesApiSubscriptions _futuresApiSubscriptions;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>? tradesChanged;

		public BinanceTradeSubscriptions(
			BinanceFuturesApiSubscriptions futuresApiSubscriptions,
			IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
			_futuresApiSubscriptions = futuresApiSubscriptions;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetTradesAsObservable()
		{
			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>((x)
				=> tradesChanged += x, (x) => tradesChanged -= x);

			return updatedApiStateNotifications;
		}

		public void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();

			_futuresApiSubscriptions.AttachSubscriptionIdToApi(api, userId, out subscribedStreamId, out var burseSession);
			burseSession.TradesChanged += OnTradesChanged;
		}

		public void UnsubscribeStream(Guid subscribedStreamId)
		{
			_futuresApiSubscriptions.DetachSubscriptionAndTryToRemoveApiSubscriptionObject(subscribedStreamId);
		}

		private void OnTradesChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs) obj)
			=> tradesChanged?.Invoke(obj);
	}
}
