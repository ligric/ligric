using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Binance.Types;
using Ligric.CryptoObserver.Extensions;
using Ligric.CryptoObserver.Interfaces;
using Newtonsoft.Json.Linq;
using Utils;
using Utils.Extensions;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesPositions : IFuturesPositions
	{
		private int eventSync = 0;

		private readonly BinanceClient _client;
		private readonly IFuturesOrdersCashed _cashedOrders;
		private readonly IFuturesLeverages _leverages;

		private readonly Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();

		internal BinanceFuturesPositions(
			BinanceClient client,
			IFuturesOrdersCashed cashedOrders,
			IFuturesLeverages leverages)
		{
			_client = client;
			_cashedOrders = cashedOrders;
			_leverages = leverages;
		}

		public ReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		internal async Task SetupPrimaryPositionsAsync(CancellationToken token)
		{
			var positionssResponse = await _client.UsdFuturesApi.Account.GetPositionInformationAsync(ct: token);

			var openPositions = positionssResponse.Data
				.Where(x => x.EntryPrice > 0)
				.ToAsyncEnumerable()
				.SelectAwaitWithCancellation(async (binancePosition, token) =>
				{
					OrderSide side = binancePosition.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
					byte? leverage = _leverages.Leverages.TryGetValue(binancePosition.Symbol, out byte leverageOut) ? leverageOut : null;

					return binancePosition.ToFuturesPositionDto(
						(long)RandomHelper.GetRandomUlong(),
						side,
						leverage,
						await GetFilledOrdersQuoteQuantity(binancePosition.Symbol, binancePosition.Quantity, binancePosition.UpdateTime, token));

				}).ForEachAwaitAsync(async position =>
				{
					lock (((ICollection)_positions).SyncRoot)
					{
						_positions.AddAndRiseEvent(this, PositionsChanged, position.Id, position, ref eventSync);
						_leverages.UpdateLeveragesFromAddedPosition(position);
					}
				}, token);
		}

		internal void OnAccountUpdated(DataEvent<BinanceFuturesStreamAccountUpdate> account)
		{
			var positions = account.Data.UpdateData.Positions;
			foreach (var position in positions)
			{
				FuturesPositionDto? existingItem = _positions.Values.FirstOrDefault(x => x.Symbol == position.Symbol);

				if (position.Quantity == 0)
				{
					if (existingItem != null && _positions.TryGetValue(existingItem.Id, out var removingPosition))
					{
						_positions.RemoveAndRiseEvent(this, PositionsChanged, removingPosition.Id, removingPosition, ref eventSync);
					}
					continue;
				}

				if (existingItem == null)
				{
					OrderSide side = position.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
					byte? leverage = _leverages.Leverages.TryGetValue(position.Symbol, out byte leverageOut) ? leverageOut : null;
					FuturesPositionDto positionDto = position.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side, leverage);
					_positions.AddAndRiseEvent(this, PositionsChanged, positionDto.Id, positionDto, ref eventSync);
					_leverages.UpdateLeveragesFromAddedPosition(positionDto);
				}
			}
		}

		private async Task<decimal> GetFilledOrdersQuoteQuantity(
			string symbol,
			decimal quantity,
			DateTime positionUpdated,
			CancellationToken token)
		{
			var orders = await GetFilledOrdersWithQuoteQuantity(symbol, quantity, positionUpdated, token);

			return orders.Sum(x => x.QuoteQuantity);
		}

		private async Task<IEnumerable<BinanceFuturesFilledOrder>> GetFilledOrdersWithQuoteQuantity(
			string symbol,
			decimal quantity,
			DateTime positionUpdated,
			CancellationToken token)
		{
			var cashedOrders = TryGetOrdersFromCash(symbol, quantity, positionUpdated, out decimal quantitySum);

			if (quantitySum == quantity)
			{
			    return cashedOrders;
			}

			var ordersCollectingResult = await GetOrdersFromBinanceAsync(
				symbol, quantity, cashedOrders.LastOrDefault()?.UpdatedTime ?? positionUpdated,
				token, quantitySum, cashedOrders);

			if (ordersCollectingResult.QuantitySum != quantity)
			{
				throw new InvalidOperationException("Position quote quantity was not finded");
			}

			return ordersCollectingResult.Orders;
		}

		private async Task<(IEnumerable<BinanceFuturesFilledOrder> Orders, decimal QuantitySum)> GetOrdersFromBinanceAsync(
			string symbol,
			decimal quantity,
			DateTime lastOrderUpdatedTime,
			CancellationToken token,
			decimal quantitySum,
			List<BinanceFuturesFilledOrder> lastFilledOrders)
		{
			while (true)
			{
				var newTime = lastOrderUpdatedTime.AddHours(-1);

				var nextFilledOrdersResponse = await _client.UsdFuturesApi.Trading.GetOrdersAsync(
					 symbol, startTime: newTime, endTime: lastOrderUpdatedTime, ct: token);

				var nextFilledOrders = nextFilledOrdersResponse.Data;

				foreach (var order in nextFilledOrders.Reverse())
				{
					if (lastFilledOrders.FirstOrDefault(x => x.Id == order.Id) != null)
					{
						continue;
					}

					if (order.Status == OrderStatus.Filled)
					{
						lastFilledOrders.Add(order.ToBinanceFuturesFilledOrder());
						quantitySum += order.LastFilledQuantity;
						if (quantity == quantitySum)
						{
							return new(lastFilledOrders, quantitySum);
						}
					}
				}
				lastOrderUpdatedTime = newTime;
			}
		}

		private List<BinanceFuturesFilledOrder> TryGetOrdersFromCash(string symbol, decimal quantity, DateTime positionUpdatedTime, out decimal quantitySum)
		{
			var cashedOrders = new List<BinanceFuturesFilledOrder>(0);
			quantitySum = 0m;

			foreach (var cashedOrder in _cashedOrders.LastFilledOrders)
			{
				if (cashedOrder.Symbol.Equals(symbol))
				{
					if (cashedOrder.UpdatedTime > positionUpdatedTime)
					{
						return cashedOrders;
					}

					quantitySum += cashedOrder.Quantity;
					cashedOrders.Add(cashedOrder);

					if (quantitySum == quantity)
					{
						break;
					}
				}
			}

			return cashedOrders;
		}
	}
}
