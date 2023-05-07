using Binance.Net.Clients;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesLastValues : IFuturesLastPrices
	{
		private readonly BinanceSocketClient _socketClient;
		private readonly IFuturesOrders _orders;
		private readonly IFuturesPositions _positions;

		private TradeDto[] _lastValues = new TradeDto[20];
		private Dictionary<string, CancellationTokenSource?> _lastValuesSubscribeCancellationTokens = new Dictionary<string, CancellationTokenSource?>();

		internal BinanceFuturesLastValues(
			BinanceSocketClient socketClient,
			IFuturesOrders orders,
			IFuturesPositions positions)
		{
			_socketClient = socketClient;
			_orders = orders;
			_positions = positions;

			_orders.OrdersChanged += OnOrdersChanged;
			_positions.PositionsChanged += OnPositionsChanged;

		}

		public TradeDto[] LastValues => _lastValues;

		public event EventHandler<TradeDto>? LastValueItemAdded;

		private async void OnOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, Core.Types.Future.FuturesOrderDto> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					//await AttachValuesSubscribeAsync(e.NewValue!.Symbol);
					break;
				case NotifyDictionaryChangedAction.Removed:
					//TryUnsubscribeValueIfDependenciesMissing(e.OldValue!.Symbol);
					break;
			}
		}

		private async void OnPositionsChanged(object sender, NotifyDictionaryChangedEventArgs<long, Core.Types.Future.FuturesPositionDto> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					//await AttachValuesSubscribeAsync(e.NewValue!.Symbol);
					break;
				case NotifyDictionaryChangedAction.Removed:
					//TryUnsubscribeValueIfDependenciesMissing(e.OldValue!.Symbol);
					break;
			}
		}
	}
}
