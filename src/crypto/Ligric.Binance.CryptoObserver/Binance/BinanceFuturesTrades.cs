using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Interfaces;
using Newtonsoft.Json.Linq;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesTrades : IFuturesTrades
	{
		private readonly BinanceSocketClient _socketClient;
		private readonly IFuturesOrders _orders;
		private readonly IFuturesPositions _positions;

		private readonly Dictionary<string, TradeDto[]> _trades = new Dictionary<string, TradeDto[]>();
		private readonly Dictionary<string, CancellationTokenSource?> _tradesSubscribeCancellationTokens = new Dictionary<string, CancellationTokenSource?>();

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

		public ReadOnlyDictionary<string, TradeDto[]> Trades => new ReadOnlyDictionary<string, TradeDto[]>(_trades);

#pragma warning disable CS0067 // The event 'BinanceFuturesTrades.TradeItemAdded' is never used
		public event EventHandler<TradeDto>? TradeItemAdded;
#pragma warning restore CS0067 // The event 'BinanceFuturesTrades.TradeItemAdded' is never used

		private async Task AttachTradesSubscribeAsync(string symbol)
		{
			bool isAdded = false;
			lock (((ICollection)_trades).SyncRoot)
			{
				if (!_trades.ContainsKey(symbol))
				{
					_trades.Add(symbol, new TradeDto[20]);
					isAdded = true;
				}
			}

			if (isAdded)
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				_tradesSubscribeCancellationTokens.Add(symbol, cancellationTokenSource);

				//await _socketClient.UsdFuturesStreams.(symbol, OnAggregatedUpdated, cancellationTokenSource.Token);
			}
		}

		private async void OnOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, Core.Types.Future.FuturesOrderDto> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					await AttachTradesSubscribeAsync(e.NewValue!.Symbol);
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
					await AttachTradesSubscribeAsync(e.NewValue!.Symbol);
					break;
				case NotifyDictionaryChangedAction.Removed:
					//TryUnsubscribeValueIfDependenciesMissing(e.OldValue!.Symbol);
					break;
			}
		}
	}
}
