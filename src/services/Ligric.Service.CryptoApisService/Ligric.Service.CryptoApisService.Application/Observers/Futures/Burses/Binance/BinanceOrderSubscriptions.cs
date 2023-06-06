 using System.Reactive.Linq;
using Ligric.Core.Types.Future;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Extensions;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinanceOrderSubscriptions : IOrderSubscribtions
	{
		private readonly IApiRepository _apiRepository;
		private readonly BinanceFuturesApiSubscriptions _futuresApiSubscriptions;

		public BinanceOrderSubscriptions(
			BinanceFuturesApiSubscriptions futuresApiSubscriptions,
			IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
			_futuresApiSubscriptions = futuresApiSubscriptions;
		}
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs)>? OrdersChanged;
		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable()
		{
			//subscribtionId = Guid.NewGuid();
			//FuturesApiSubscribtionsObserver? subscribedApi = null;
			//lock (subLock)
			//{
			//	var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
			//	CreateSubscritionsClientIfNotExitstsAndAddToSharedCollection(userId, api, out subscribtionId, out subscribedApi);
			//	subscribedApi.OrdersChanged += OnOrdersChanged;
			//}

			//exchangedId = subscribedApi.ExchangeId;

			//var oldOrders = subscribedApi.FuturesClient.Orders.Orders.Select(
			//	x => (subscribedApi.ExchangeId, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(x.Key, x.Value, 0, 0))).ToObservable();

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> OrdersChanged += x, (x) => OrdersChanged -= x);

			return /*oldOrders.Concat(updatedApiStateNotifications)*/updatedApiStateNotifications;
		}

		public void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();

			_futuresApiSubscriptions.AttachSubscriptionIdToApi(api, userId, out subscribedStreamId, out var burseSession);
			burseSession.OrdersChanged += OnOrdersChanged;
		}

		public void UnsubscribeStream(Guid subscribedStreamId)
		{
			_futuresApiSubscriptions.DetachSubscribtionAndTryToRemoveApiSubscribtionObject(subscribedStreamId);
		}

		private void OnOrdersChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs) obj)
			=> OrdersChanged?.Invoke(obj);
	}
}
