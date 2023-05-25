using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Binance.Types;
using Ligric.CryptoObserver.Extensions;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesOrders : IFuturesOrders, IFuturesOrdersCashed
	{
		private int eventSync = 0;

		private readonly BinanceClient _client;

		private readonly Dictionary<long, FuturesOrderDto> _orders = new Dictionary<long, FuturesOrderDto>();

		public readonly List<BinanceFuturesFilledOrder> _lastFilledOrders = new List<BinanceFuturesFilledOrder>();

		internal BinanceFuturesOrders(BinanceClient client)
		{
			_client = client;
		}

		public ReadOnlyCollection<BinanceFuturesFilledOrder> LastFilledOrders => new ReadOnlyCollection<BinanceFuturesFilledOrder>(_lastFilledOrders);

		public ReadOnlyDictionary<long, FuturesOrderDto> Orders => new ReadOnlyDictionary<long, FuturesOrderDto>(_orders);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OrdersChanged;

		internal void OnOrdersUpdated(DataEvent<BinanceFuturesStreamOrderUpdate> order)
		{
			BinanceFuturesStreamOrderUpdateData streamOrder = order.Data.UpdateData;

			// Adding
			if (streamOrder.Status is OrderStatus.New)
			{
				FuturesOrderDto orderDto = streamOrder.ToFuturesOrderDto();
				_orders.AddAndRiseEvent(this, OrdersChanged, orderDto.Id, orderDto, ref eventSync);
				return;
			}

			// Removing
			if (_orders.TryGetValue(streamOrder.OrderId, out var removingOrder))
			{
				_orders.RemoveAndRiseEvent(this, OrdersChanged, removingOrder.Id, removingOrder, ref eventSync);
			}

			if (streamOrder.Status == OrderStatus.Filled)
			{
				var ordersHistoryResponse = _client.UsdFuturesApi.Trading.GetOrderAsync(streamOrder.Symbol, streamOrder.OrderId)
					.ContinueWith(response =>
					{
						lock (((ICollection)_lastFilledOrders).SyncRoot)
						{
							var newOrder = response.Result.Data.ToBinanceFuturesFilledOrder();
							if (!_lastFilledOrders.Contains(newOrder))
							{
								_lastFilledOrders.Add(newOrder);
							}
						}
					});
			}
		}

		internal async Task SetupPrimaryOrdersAsync(CancellationToken token)
		{
			// TODO : Temporary
			if (_orders.Count > 0)
			{
				_orders.ClearAndRiseEvent(this, OrdersChanged, ref eventSync);
			}

			var ordersResponse = await _client.UsdFuturesApi.Trading.GetOpenOrdersAsync(ct: token);
			var orders = ordersResponse.Data;

			lock (((ICollection)_orders).SyncRoot)
			{
				foreach (var order in orders)
				{
					_orders.AddAndRiseEvent(this, OrdersChanged, order.Id, order.ToFuturesOrderDto(), ref eventSync);
					if (order.Status == OrderStatus.Filled)
					{
						lock (((ICollection)_lastFilledOrders).SyncRoot)
						{
							var newOrder = order.ToBinanceFuturesFilledOrder();
							if (!_lastFilledOrders.Contains(newOrder))
							{
								_lastFilledOrders.Add(order.ToBinanceFuturesFilledOrder());
							}
						}
					}
				}
			}
		}
	}
}
