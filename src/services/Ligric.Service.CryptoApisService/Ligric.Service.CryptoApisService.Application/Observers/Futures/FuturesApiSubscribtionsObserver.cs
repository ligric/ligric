using System.Collections.Generic;
using System.Collections.ObjectModel;
using Binance.Net.Objects;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures
{
	public class FuturesApiSubscribtionsObserver
	{
		private readonly Dictionary<Guid, long> _usersSubscribtions = new Dictionary<Guid, long>();

		public Guid ExchangeId { get; init; }

		public ApiDto Api { get; init; }

		public IFuturesClient FuturesClient { get; init; }


		public FuturesApiSubscribtionsObserver(ApiDto api, BinanceApiCredentials credentials, bool isTest = true)
		{
			ExchangeId = Guid.NewGuid();
			Api = api;
			FuturesClient = new BinanceFuturesClient(credentials, isTest);
			FuturesClient.Orders.OrdersChanged += OnOrdersChanged;
			FuturesClient.Trades.ValuesChanged += OnValuesChanged;
			FuturesClient.Positions.PositionsChanged += OnPositionsChanged;
			FuturesClient.Leverages.LeveragesChanged += OnLeveragesChanged;
			FuturesClient.StartStreamAsync();
		}

		/// <summary>
		/// Key subscribtion Id
		/// Value user Id
		/// </summary>
		public ReadOnlyDictionary<Guid, long> UsersSubscribtions => new ReadOnlyDictionary<Guid, long>(_usersSubscribtions);

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs)>? OrdersChanged;

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal>)>? ValuesChanged;

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs)>? PositionsChanged;

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> leverageEventArgs)>? LeveragesChanged;

		public Guid CreateUserSubscribtionIdAndAddToPull(long userId)
		{
			var subscribtionid = Guid.NewGuid();
			_usersSubscribtions.Add(subscribtionid, userId);
			return subscribtionid;
		}

		public bool TryRemoveUserSubscribtion(Guid userSubscribtion, out long userId)
		{
			return _usersSubscribtions.Remove(userSubscribtion, out userId);
		}

		public void RemoveUserSubscribtions(long userId)
		{
			foreach (var item in _usersSubscribtions.Where(x => x.Value == userId))
			{
				_usersSubscribtions.Remove(item.Key);
			}
		}

		public void Dispose()
		{
			FuturesClient.Orders.OrdersChanged -= OnOrdersChanged;
			FuturesClient.Trades.ValuesChanged -= OnValuesChanged;
			FuturesClient.Positions.PositionsChanged -= OnPositionsChanged;
			FuturesClient.Leverages.LeveragesChanged -= OnLeveragesChanged;
			FuturesClient.StopStream();
			FuturesClient.Dispose();
		}

		private void OnValuesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> valueEventArgs)
			=> ValuesChanged?.Invoke((ExchangeId, valueEventArgs));

		private void OnOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> ordersChangedEventArgs)
			=> OrdersChanged?.Invoke((ExchangeId, ordersChangedEventArgs));

		private void OnPositionsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> positionsChangedEventArgs)
			=> PositionsChanged?.Invoke((ExchangeId, positionsChangedEventArgs));

		private void OnLeveragesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, byte> leveragesChangedEventArgs)
			=> LeveragesChanged?.Invoke((ExchangeId, leveragesChangedEventArgs));
	}
}
