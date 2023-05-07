using Binance.Net.Clients;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesTrades : IFuturesTrades
	{
		private readonly BinanceSocketClient _socketClient;
		private readonly IFuturesOrders _orders;
		private readonly IFuturesPositions _positions;

		private TradeDto[] _trades = new TradeDto[20];
		private Dictionary<string, CancellationTokenSource?> _tradesSubscribeCancellationTokens = new Dictionary<string, CancellationTokenSource?>();

		internal BinanceFuturesTrades(
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

		public TradeDto[] Trades => _trades;

		public event EventHandler<TradeDto>? TradeItemAdded;

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
