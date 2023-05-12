using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Ligric.CryptoObserver.Interfaces;
using Utils;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Sockets;
using System.Collections;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesValues : IFuturesValues
	{
		private int eventSync = 0;

		private readonly BinanceSocketClient _socketClient;
		private readonly IFuturesOrders _orders;
		private readonly IFuturesPositions _positions;

		private readonly Dictionary<string, decimal> _values = new Dictionary<string, decimal>();
		private readonly Dictionary<string, CancellationTokenSource?> _valuesSubscribeCancellationTokens = new Dictionary<string, CancellationTokenSource?>();

		internal BinanceFuturesValues(
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

		public ReadOnlyDictionary<string, decimal> Values => new ReadOnlyDictionary<string, decimal>(_values);

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;

		private async Task AttachValuesSubscribeAsync(string symbol)
		{
			bool isAdded = false;
			lock (((ICollection)_values).SyncRoot)
			{
				if (!_values.ContainsKey(symbol))
				{
					_values.Add(symbol, -1);
					isAdded = true;
				}
			}

			if (isAdded)
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				_valuesSubscribeCancellationTokens.Add(symbol, cancellationTokenSource);

				await _socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(symbol, OnAggregatedUpdated, cancellationTokenSource.Token);
			}
		}

		private void UnsubscribeValue(string symbol)
		{
			if (_valuesSubscribeCancellationTokens.TryGetValue(symbol, out var cancellationTokenSource))
			{
				_valuesSubscribeCancellationTokens.Remove(symbol);
				cancellationTokenSource?.Cancel();
				cancellationTokenSource?.Dispose();
			}
			_values.Remove(symbol);
		}

		private void TryUnsubscribeValueIfDependenciesMissing(string symbol)
		{
			if (_orders.Orders.Values.FirstOrDefault(x => x.Symbol == symbol) == null
				&& _positions.Positions.Values.FirstOrDefault(x => x.Symbol == symbol) == null)
			{
				lock (((ICollection)_values).SyncRoot)
				{
					lock (((ICollection)_valuesSubscribeCancellationTokens).SyncRoot)
					{
						UnsubscribeValue(symbol);
					}
				}
			}
		}

		private void OnAggregatedUpdated(DataEvent<BinanceStreamAggregatedTrade> obj)
		{
			var data = obj.Data;

			if (!_valuesSubscribeCancellationTokens.TryGetValue(data.Symbol, out var valueCancelationToken)
				|| valueCancelationToken == null || valueCancelationToken.IsCancellationRequested)
			{
				return;
			}

			_values.SetAndRiseEvent(this, ValuesChanged, data.Symbol, data.Price, ref eventSync);
		}

		private async void OnOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, Core.Types.Future.FuturesOrderDto> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					await AttachValuesSubscribeAsync(e.NewValue!.Symbol);
					break;
				case NotifyDictionaryChangedAction.Removed:
					TryUnsubscribeValueIfDependenciesMissing(e.OldValue!.Symbol);
					break;
			}
		}

		private async void OnPositionsChanged(object sender, NotifyDictionaryChangedEventArgs<long, Core.Types.Future.FuturesPositionDto> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					await AttachValuesSubscribeAsync(e.NewValue!.Symbol);
					break;
				case NotifyDictionaryChangedAction.Removed:
					TryUnsubscribeValueIfDependenciesMissing(e.OldValue!.Symbol);
					break;
			}
		}
	}
}
