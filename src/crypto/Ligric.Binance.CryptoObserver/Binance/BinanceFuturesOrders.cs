using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Sockets;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Extensions;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesOrders : IFuturesOrders
	{
		private int eventSync = 0;

		private readonly BinanceClient _client;

		private Dictionary<long, FuturesOrderDto> _orders = new Dictionary<long, FuturesOrderDto>();

		internal BinanceFuturesOrders(BinanceClient client)
		{
			_client = client;
		}

		public ReadOnlyDictionary<long, FuturesOrderDto> Orders => new ReadOnlyDictionary<long, FuturesOrderDto>(_orders);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OrdersChanged;

		internal void OnOrdersUpdated(DataEvent<BinanceFuturesStreamOrderUpdate> order)
		{
			BinanceFuturesStreamOrderUpdateData streamOrder = order.Data.UpdateData;
			FuturesOrderDto orderDto = streamOrder.ToFuturesOrderDto();

			if (streamOrder.Status is OrderStatus.New)
			{
				_orders.AddAndRiseEvent(this, OrdersChanged, orderDto.Id, orderDto, ref eventSync);
				return;
			}
			_orders.RemoveAndRiseEvent(this, OrdersChanged, orderDto.Id, ref eventSync);
		}

		internal async Task SetupPrimaryOrdersAsync(CancellationToken token)
		{
			var ordersResponse = await _client.UsdFuturesApi.Trading.GetOpenOrdersAsync(ct: token);
			var orders = ordersResponse.Data
				.Select(binanceOrder => binanceOrder.ToFuturesOrderDto())
				.ToList();

			lock (((ICollection)_orders).SyncRoot)
			{
				foreach (var order in orders)
				{
					_orders.AddAndRiseEvent(this, OrdersChanged, order.Id, order, ref eventSync);
				}
			}
		}
	}
}
