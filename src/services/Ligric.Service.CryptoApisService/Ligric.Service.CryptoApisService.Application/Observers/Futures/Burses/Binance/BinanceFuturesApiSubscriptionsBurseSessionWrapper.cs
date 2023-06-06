using Binance.Net.Objects;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinanceFuturesApiSubscriptionsBurseSessionWrapper : IFuturesApiSubscriptionsBurseSessionWrapper
	{
		public Guid BurseSessionId { get; }

		public ApiDto Api { get; }

		public IFuturesClient FuturesClient { get; }

		public BinanceFuturesApiSubscriptionsBurseSessionWrapper(ApiDto api, BinanceApiCredentials credentials, bool isTest = true)
		{
			BurseSessionId = Guid.NewGuid();
			Api = api;
			FuturesClient = new BinanceFuturesClient(credentials, isTest);
			FuturesClient.Orders.OrdersChanged += OnOrdersChanged;
			FuturesClient.Trades.ValuesChanged += OnValuesChanged;
			FuturesClient.Positions.PositionsChanged += OnPositionsChanged;
			FuturesClient.Leverages.LeveragesChanged += OnLeveragesChanged;
			FuturesClient.StartStreamAsync();
		}

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs)>? OrdersChanged;

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal>)>? TradesChanged;

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs)>? PositionsChanged;

		public event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> leverageEventArgs)>? LeveragesChanged;

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
			=> TradesChanged?.Invoke((BurseSessionId, valueEventArgs));

		private void OnOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> ordersChangedEventArgs)
			=> OrdersChanged?.Invoke((BurseSessionId, ordersChangedEventArgs));

		private void OnPositionsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> positionsChangedEventArgs)
			=> PositionsChanged?.Invoke((BurseSessionId, positionsChangedEventArgs));

		private void OnLeveragesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, byte> leveragesChangedEventArgs)
			=> LeveragesChanged?.Invoke((BurseSessionId, leveragesChangedEventArgs));
	}
}
