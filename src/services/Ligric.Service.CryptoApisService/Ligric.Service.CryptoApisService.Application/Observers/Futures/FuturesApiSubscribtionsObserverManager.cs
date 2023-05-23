using Ligric.Core.Types.Future;
using System.Reactive.Linq;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Extensions;
using Utils;
using Binance.Net.Objects;
using Ligric.Core.Ligric.Core.Types.Api;
using System.Collections;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures
{
	public class FuturesApiSubscribtionsObserverManager : IFuturesApiSubscribtionsObserverManager
	{
		private readonly IApiRepository _apiRepository;
		private static readonly Object subLock = new Object();

		private static readonly Dictionary<long, FuturesApiSubscribtionsObserver> subscribedApis = new Dictionary<long, FuturesApiSubscribtionsObserver>();

		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>? OrdersChanged;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>? ValuesChanged;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>? PositionsChanged;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)>? LaveragesChanged;

		public FuturesApiSubscribtionsObserverManager(
			IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId)
		{
			FuturesApiSubscribtionsObserver? subscribedApi = null;
			lock (subLock)
			{
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				CreateSubscritionsClientIfNotExitstsAndAddToSharedCollection(userId, api, out subscribtionId, out subscribedApi);
				subscribedApi.OrdersChanged += OnOrdersChanged;
			}

			exchangedId = subscribedApi.ExchangeId;

			var oldOrders = subscribedApi.FuturesClient.Orders.Orders.Select(
				x => (subscribedApi.ExchangeId, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(x.Key, x.Value, 0, 0))).ToObservable();

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> OrdersChanged += x, (x) => OrdersChanged -= x);

			return oldOrders.Concat(updatedApiStateNotifications);
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetValuesAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId)
		{
			FuturesApiSubscribtionsObserver? subscribedApi = null;
			lock (subLock)
			{
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				CreateSubscritionsClientIfNotExitstsAndAddToSharedCollection(userId, api, out subscribtionId, out subscribedApi);
				subscribedApi.ValuesChanged += OnValuesChanged;
			}

			exchangedId = subscribedApi.ExchangeId;

			var oldTrades = subscribedApi.FuturesClient.Trades.Values.Select(
				x => (subscribedApi.ExchangeId, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(x.Key, x.Value, 0, 0))).ToObservable();

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)>((x)
				=> ValuesChanged += x, (x) => ValuesChanged -= x);

			return oldTrades.Concat(updatedApiStateNotifications);
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId)
		{
			FuturesApiSubscribtionsObserver? subscribedApi = null;
			lock (subLock)
			{
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				CreateSubscritionsClientIfNotExitstsAndAddToSharedCollection(userId, api, out subscribtionId, out subscribedApi);
				subscribedApi.PositionsChanged += OnPositionsChanged;
			}

			exchangedId = subscribedApi.ExchangeId;

			var oldPositions = subscribedApi.FuturesClient.Positions.Positions.Select(
				x => (subscribedApi.ExchangeId, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(x.Key, x.Value, 0, 0))).ToObservable();
			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>((x)
				=> PositionsChanged += x, (x) => PositionsChanged -= x);

			return oldPositions.Concat(updatedApiStateNotifications);
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)> GetLeveragesAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId)
		{
			FuturesApiSubscribtionsObserver? subscribedApi = null;
			lock (subLock)
			{
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				CreateSubscritionsClientIfNotExitstsAndAddToSharedCollection(userId, api, out subscribtionId, out subscribedApi);
				subscribedApi.LeveragesChanged += OnLeveragesChanged;
			}

			exchangedId = subscribedApi.ExchangeId;

			var oldLeverages = subscribedApi.FuturesClient.Leverages.Leverages.Select(
				x => (subscribedApi.ExchangeId, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(x.Key, x.Value, 0, 0))).ToObservable();

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)>((x)
				=> LaveragesChanged += x, (x) => LaveragesChanged -= x);
			return oldLeverages.Concat(updatedApiStateNotifications);
		}

		/// <summary>
		/// Will unsubscribe subscribtion from his Id.
		/// Will remove the <see cref="FuturesApiSubscribtionsObserver"/> if noone subscribed to this API/>
		/// </summary>
		public void UnsubscribeIdAndTryToRemoveApiSubscribtionObject(Guid subscribtionId)
		{
			try
			{
				foreach (var subscribedApi in subscribedApis.Values)
				{
					if (subscribedApi.UsersSubscribtions.ContainsKey(subscribtionId))
					{
						lock (((ICollection)subscribedApi.UsersSubscribtions).SyncRoot)
						{
							subscribedApi.TryRemoveUserSubscribtion(subscribtionId, out long userId);
							if (subscribedApi.UsersSubscribtions.Count == 0)
							{
								lock (((ICollection)subscribedApis).SyncRoot)
								{
									subscribedApis.Remove(userId);
									subscribedApi.Dispose();
									System.Diagnostics.Debug.WriteLine($"Api {userId} was fully removed.");
								}
							}
						}
						return;
					}
				}
			}
			catch(Exception ex)
			{
				var test = ex;
			}

		}

		private void CreateSubscritionsClientIfNotExitstsAndAddToSharedCollection(long userId, ApiDto api, out Guid subscribtionId, out FuturesApiSubscribtionsObserver subscribedApi)
		{
			subscribedApi = subscribedApis.Values.FirstOrDefault(x => x.Api == api);

			if (subscribedApi != null)
			{
				subscribtionId = subscribedApi.CreateUserSubscribtionIdAndAddToPull(userId);
			}
			else
			{
				subscribedApi = new FuturesApiSubscribtionsObserver(api, new BinanceApiCredentials(api.PublicKey, api.PrivateKey));
				subscribtionId = subscribedApi.CreateUserSubscribtionIdAndAddToPull(userId);
				subscribedApis.Add(api.Id, subscribedApi);
			}
		}

		private void OnPositionsChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs) obj)
			=> PositionsChanged?.Invoke(obj);

		private void OnValuesChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal>) obj)
			=> ValuesChanged?.Invoke(obj);

		private void OnOrdersChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs) obj)
			=> OrdersChanged?.Invoke(obj);

		private void OnLeveragesChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> leverageEventArgs) obj)
			=> LaveragesChanged?.Invoke(obj);
	}
}
