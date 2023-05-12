﻿using Utils;
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
			public Guid ExchangeId { get; init; }

			public ApiDto Api { get; init; }

			public IFuturesClient FuturesClient { get; init; }

			public TemporaryApiSubscriptions(ApiDto api, BinanceApiCredentials credentials, bool isTest = true)
			{
				ExchangeId = Guid.NewGuid();
				Api = api;
				FuturesClient = new BinanceFuturesClient(credentials, isTest);
				FuturesClient.Orders.OrdersChanged += OnOrdersChanged;
				FuturesClient.Values.ValuesChanged += OnValuesChanged;
				FuturesClient.Positions.PositionsChanged += OnPositionsChanged;
				FuturesClient.Leverages.LeveragesChanged += OnLeveragesChanged;
				FuturesClient.StartStreamAsync();
			}

			public Dictionary<Guid, long> UsersSubscribtions = new Dictionary<Guid, long>();

			public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs)>? OrdersChanged;

			public event Action<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;

			public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs)>? PositionsChanged;

			public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> leverageEventArgs)>? LeveragesChanged;

			private void OnValuesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> valueEventArgs)
				=> ValuesChanged?.Invoke((valueEventArgs));

			private void OnOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> ordersChangedEventArgs)
				=> OrdersChanged?.Invoke((ExchangeId,  ordersChangedEventArgs));

			private void OnPositionsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> positionsChangedEventArgs)
				=> PositionsChanged?.Invoke((ExchangeId, positionsChangedEventArgs));

			private void OnLeveragesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, byte> leveragesChangedEventArgs)
				=> LeveragesChanged?.Invoke((ExchangeId, leveragesChangedEventArgs));
		}

		private readonly IApiRepository _apiRepository;

		private static readonly Object subLock = new Object();
		private static readonly IList<TemporaryApiSubscriptions> subscribedApis = new List<TemporaryApiSubscriptions>();

		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>? OrdersChanged;
		private event Action<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>? PositionsChanged;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)>? LaveragesChanged;

		public TemporaryUserFuturesObserver(IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId, out Guid subscribtionId)
		{
			lock (subLock){
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				SubscribeUser(userId, api, out subscribtionId);
			}

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)>((x)
				=> OrdersChanged += x, (x) => OrdersChanged -= x);
			return updatedApiStateNotifications;
		}

		public IObservable<NotifyDictionaryChangedEventArgs<string, decimal>> GetValuesAsObservable(long userId, long userApiId, out Guid subscribtionId)
		{
			lock (subLock) {
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				SubscribeUser(userId, api, out subscribtionId);
			}

			var updatedApiStateNotifications = Observable.FromEvent<NotifyDictionaryChangedEventArgs<string, decimal>>((x)
				=> ValuesChanged += x, (x) => ValuesChanged -= x);
			return updatedApiStateNotifications;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId, out Guid subscribtionId)
		{
			lock (subLock){
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				SubscribeUser(userId, api, out subscribtionId);
			}

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)>((x)
				=> PositionsChanged += x, (x) => PositionsChanged -= x);

			return updatedApiStateNotifications;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)> GetLeveragesAsObservable(long userId, long userApiId, out Guid subscribtionId)
		{
			lock (subLock)
			{
				var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();
				SubscribeUser(userId, api, out subscribtionId);
			}

			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)>((x)
				=> LaveragesChanged += x, (x) => LaveragesChanged -= x);
			return updatedApiStateNotifications;
		}

		private void SubscribeUser(long userId, ApiDto api, out Guid subscribtionId)
		{
			TemporaryApiSubscriptions? subscribedApi = subscribedApis.FirstOrDefault(x => x.Api == api);

			if (subscribedApi != null)
			{
				subscribtionId = Guid.NewGuid();
				subscribedApi.UsersSubscribtions.Add(subscribtionId, userId);
			}
			else
			{
				subscribedApi = new TemporaryApiSubscriptions(api, new BinanceApiCredentials(api.PublicKey, api.PrivateKey));
				subscribtionId = Guid.NewGuid();
				subscribedApi.UsersSubscribtions.Add(subscribtionId, userId);
				subscribedApi.OrdersChanged += OnOrdersChanged;
				subscribedApi.ValuesChanged += OnValuesChanged;
				subscribedApi.PositionsChanged += OnPositionsChanged;
				subscribedApi.LeveragesChanged += OnLeveragesChanged;
				subscribedApis.Add(subscribedApi);
			}
		}

		private void OnPositionsChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs) obj)
			=> PositionsChanged?.Invoke(obj);

		private void OnValuesChanged(NotifyDictionaryChangedEventArgs<string, decimal> obj)
			=> ValuesChanged?.Invoke(obj);

		private void OnOrdersChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs) obj)
			=> OrdersChanged?.Invoke(obj);

		private void OnLeveragesChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> leverageEventArgs) obj)
			=> LaveragesChanged?.Invoke(obj);

	}
}
