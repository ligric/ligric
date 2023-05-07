using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesLastValues : IFuturesLastPrices
	{
		private readonly BinanceSocketClient _socketClient;
		private readonly IFuturesOrders _orders;
		private readonly IFuturesPositions _positions;

		private Dictionary<string, decimal> _lastValues = new Dictionary<string, decimal>();
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

		public ReadOnlyDictionary<string, decimal> LastValues => new ReadOnlyDictionary<string, decimal>(_lastValues);

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? LastValuesChanged;

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
