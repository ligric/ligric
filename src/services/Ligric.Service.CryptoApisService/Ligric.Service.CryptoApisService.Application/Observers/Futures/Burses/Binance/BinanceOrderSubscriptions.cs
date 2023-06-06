﻿ using System.Reactive.Linq;
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
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>? ordersChanged;

		public BinanceOrderSubscriptions(
			BinanceFuturesApiSubscriptions futuresApiSubscriptions,
			IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
			_futuresApiSubscriptions = futuresApiSubscriptions;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable()
		{
			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> ordersChanged += x, (x) => ordersChanged -= x);

			return updatedApiStateNotifications;
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

		private void OnOrdersChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs) obj)
			=> ordersChanged?.Invoke(obj);
	}
}
