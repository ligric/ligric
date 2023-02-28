using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Ligric.Domain.Types.Api;
using Ligric.Domain.Types.Future;
using Ligric.Domain.Types.User;
using Ligric.Server.Domain.Entities.Apis;
using Ligric.Server.Domain.Entities.UserApies;
using Ligric.Server.Domain.TypeExtensions;
using Utils;

namespace Ligric.Application.Orders
{
	public class OrdersObserver : IOrdersObserverManager
	{
		private readonly IUserApiRepository _userApiRepository;
		private readonly IApiRepository _apiRepository;

		private readonly Dictionary<ApiDto, IList<long>> subscribedOrders = new Dictionary<ApiDto, IList<long>>();

		private event Action<(EventAction Action, long UserId, FuturesOrderDto order)>? OrdersChanged;

		public OrdersObserver(
			IUserApiRepository userApiRepository,
			IApiRepository apiRepository)
		{
			_userApiRepository = userApiRepository;
			_apiRepository = apiRepository;
		}

		public IObservable<(EventAction Action, long UserId, FuturesOrderDto order)> GetOrdersAsObservable(long userId, long userApiId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();

			TryAddUserIdToSubscribedOrders(userId, api);

			var updatedApiStateNotifications = Observable.FromEvent<(EventAction Action, long UserId, FuturesOrderDto order)>((x) => OrdersChanged += x, (x) => OrdersChanged -= x);

			return updatedApiStateNotifications;
		}

		private bool TryAddUserIdToSubscribedOrders(long userId, ApiDto api)
		{
			if (subscribedOrders.TryGetValue(api, out var userIds))
			{
				if (!userIds.Contains(userId))
				{
					userIds.Add(userId);
					return true;
				}
			}
			else
			{
				subscribedOrders.Add(api, new List<long> { userId });
				return true;
			}

			return false;
		}
	}
}
