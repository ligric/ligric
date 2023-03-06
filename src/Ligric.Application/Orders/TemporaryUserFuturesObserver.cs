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

			public event Action<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> valueEventArgs)>? ValuesChanged;

			public event Action<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs)>? PositionsChanged;

			public TemporaryApiSubscriptions(ApiDto api, BinanceApiCredentials credentials, bool isTest = true)
			{
				Api = api;
				FuturesManager = new BinanceFuturesManager(credentials, isTest);
				FuturesManager.OrdersChanged += OnOrdersChanged;
				FuturesManager.ValuesChanged += OnValuesChanged;
				FuturesManager.PositionsChanged += OnPositionsChanged;
				_ = FuturesManager.AttachOrdersSubscribtionsAsync();
			}

			private void OnValuesChanged(object sender, NotifyDictionaryChangedEventArgs<string, decimal> valueEventArgs)
			{
				ValuesChanged?.Invoke((UserIds, valueEventArgs));
			}

			private void OnOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> ordersChangedEventArgs)
			{
				OrdersChanged?.Invoke((UserIds, ordersChangedEventArgs));
			}

			private void OnPositionsChanged(object sender, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> positionsChangedEventArgs)
			{
				PositionsChanged?.Invoke((UserIds, positionsChangedEventArgs));
			}
		}

		private readonly IUserApiRepository _userApiRepository;
		private readonly IApiRepository _apiRepository;

		private readonly IList<TemporaryApiSubscriptions> subscribedApis = new List<TemporaryApiSubscriptions>();

		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>? OrdersChanged;
		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>? ValuesChanged;
		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>? PositionsChanged;

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
			TryAddUserIdToSubscrions(userId, api, out TemporaryApiSubscriptions? subscribedApi);

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> OrdersChanged += x, (x) => OrdersChanged -= x);
			return updatedApiStateNotifications;
		}

		public IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetValuesAsObservable(long userId, long userApiId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
			TryAddUserIdToSubscrions(userId, api, out TemporaryApiSubscriptions? subscribedApi);

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>((x)
				=> ValuesChanged += x, (x) => ValuesChanged -= x);
			return updatedApiStateNotifications;
		}

		public IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
			TryAddUserIdToSubscrions(userId, api, out TemporaryApiSubscriptions? subscribedApi);

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>((x)
				=> PositionsChanged += x, (x) => PositionsChanged -= x);
			return updatedApiStateNotifications;
		}

		private bool TryAddUserIdToSubscrions(long userId, ApiDto api, out TemporaryApiSubscriptions? subscribedApi)
		{
			subscribedApi = subscribedApis.FirstOrDefault(x => x.Api == api);

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
				subscribedApi = new TemporaryApiSubscriptions(api, new BinanceApiCredentials(api.PublicKey, api.PrivateKey));
				subscribedApi.UserIds.Add(userId);
				subscribedApi.OrdersChanged += OnOrdersChanged;
				subscribedApi.ValuesChanged += OnValuesChanged;
				subscribedApi.PositionsChanged += OnPositionsChanged;
				subscribedApis.Add(subscribedApi);
				return true;
			}

			return false;
		}

		private void OnPositionsChanged((IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs) obj)
		{
			PositionsChanged?.Invoke(obj);
		}

		private void OnValuesChanged((IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> valueEventArgs) obj)
		{
			ValuesChanged?.Invoke(obj);
		}

		private void OnOrdersChanged((IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs) obj)
		{
			OrdersChanged?.Invoke(obj);
		}
	}
}
