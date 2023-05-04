using Utils;
using Binance.Net.Objects;
using System.Reactive.Linq;
using Ligric.CryptoObserver;
using Ligric.Core.Types.Future;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Service.CryptoApisService.Domain.Extensions;
using Ligric.Service.CryptoApisService.Application.Repositories;

namespace Ligric.Service.CryptoApisService.Application
{
	public class TemporaryUserFuturesObserver : ITemporaryUserFuturesObserver
	{
		public record TemporaryApiSubscriptions
		{
			public ApiDto Api { get; init; }

			public IFuturesManager FuturesManager { get; init; }

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

			private void OnValuesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> valueEventArgs)
			{
				ValuesChanged?.Invoke((UserIds, valueEventArgs));
			}

			private void OnOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> ordersChangedEventArgs)
			{
				OrdersChanged?.Invoke((UserIds, ordersChangedEventArgs));
			}

			private void OnPositionsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> positionsChangedEventArgs)
			{
				PositionsChanged?.Invoke((UserIds, positionsChangedEventArgs));
			}
		}

		private readonly IApiRepository _apiRepository;

		private static readonly Object subLock = new Object();
		private static readonly IList<TemporaryApiSubscriptions> subscribedApis = new List<TemporaryApiSubscriptions>();

		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>? OrdersChanged;
		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>? ValuesChanged;
		private event Action<(IEnumerable<long> UserId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>? PositionsChanged;

		public TemporaryUserFuturesObserver(IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
		}

		public IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId)
		{
			lock (subLock){
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				TryAddUserIdToSubscrions(userId, api, out var subscribedApi);
			}

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> OrdersChanged += x, (x) => OrdersChanged -= x);
			return updatedApiStateNotifications;
		}

		public IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetValuesAsObservable(long userId, long userApiId)
		{
			lock (subLock) {
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				TryAddUserIdToSubscrions(userId, api, out var subscribedApi);
			}

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>((x)
				=> ValuesChanged += x, (x) => ValuesChanged -= x);
			return updatedApiStateNotifications;
		}

		public IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId)
		{
			lock (subLock){
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				TryAddUserIdToSubscrions(userId, api, out var subscribedApi);
			}

			var updatedApiStateNotifications = Observable.FromEvent<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>((x)
				=> PositionsChanged += x, (x) => PositionsChanged -= x);
			return updatedApiStateNotifications;
		}

		private bool TryAddUserIdToSubscrions(long userId, ApiDto api, out TemporaryApiSubscriptions? subscribedApi)
		{
			subscribedApi = subscribedApis.FirstOrDefault(x => x.Api == api);

			if (subscribedApi != null)
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
