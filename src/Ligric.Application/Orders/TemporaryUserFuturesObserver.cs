using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Binance.Net.Objects;
using Ligric.CryptoObserver;
using Ligric.Domain.Types.Api;
using Ligric.Domain.Types.Future;
using Ligric.Server.Domain.Entities.Apis;
using Ligric.Server.Domain.Entities.UserApies;
using Ligric.Server.Domain.TypeExtensions;
using Utils;

namespace Ligric.Application.Orders
{
	public class TemporaryUserFuturesObserver : ITemporaryUserFuturesObserver
	{
		public record TemporaryApiSubscriptions
		{
			public ApiDto Api { get; init; }

			public BinanceFuturesManager FuturesManager { get; init; }

			public IList<long> UserIds { get; } = new List<long>();

			public event Action<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs)>? OrdersChanged;

			public TemporaryApiSubscriptions(ApiDto api, BinanceApiCredentials credentials, bool isTest = true)
			{
				Api = api;
				FuturesManager = new BinanceFuturesManager(credentials, isTest);
				FuturesManager.OrdersChanged += OnOrdersChanged;
			}

			private void OnOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> ordersChangedEventArgs)
			{
				OrdersChanged?.Invoke((UserIds, ordersChangedEventArgs));
			}
		}

		private readonly IUserApiRepository _userApiRepository;
		private readonly IApiRepository _apiRepository;

		private readonly IList<TemporaryApiSubscriptions> subscribedApis = new List<TemporaryApiSubscriptions>();

		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>? OrdersChanged;

		public TemporaryUserFuturesObserver(
			IUserApiRepository userApiRepository,
			IApiRepository apiRepository)
		{
			_userApiRepository = userApiRepository;
			_apiRepository = apiRepository;
		}

		public IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
			TryAddUserIdToSubscribedOrders(userId, api);

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> OrdersChanged += x, (x) => OrdersChanged -= x);
			return updatedApiStateNotifications;
		}

		private bool TryAddUserIdToSubscribedOrders(long userId, ApiDto api)
		{
			var subscribedApi = subscribedApis.FirstOrDefault(x => x.Api == api);

			if(subscribedApi != null)
			{
				long? subscribedUser = subscribedApi.UserIds.FirstOrDefault(subscribedUserId => subscribedUserId == userId);
				if (subscribedUser == null)
				{
					subscribedApi.UserIds.Add(userId);
					return true;
				}
			}
			else
			{
				var apiSubscriptions = new TemporaryApiSubscriptions(api, new BinanceApiCredentials(api.PublicKey, api.PrivateKey));
				apiSubscriptions.UserIds.Add(userId);
				apiSubscriptions.OrdersChanged += OnOrdersChanged;
				subscribedApis.Add(apiSubscriptions);
				return true;
			}

			return false;
		}

		private void OnOrdersChanged((IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs) obj)
		{
			OrdersChanged?.Invoke(obj);
		}
	}
}
