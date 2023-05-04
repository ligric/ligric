using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Futures.Socket;
using Ligric.Core.Types.Future;

namespace Ligric.CryptoObserver.Extensions
{
	public static class OrderTypeExtensions
	{
		public static FuturesOrderDto ToFuturesOrderDto(this BinanceFuturesOrder binanceOrder)
			=> new FuturesOrderDto(
				binanceOrder.Id,
				binanceOrder.Symbol,
				binanceOrder.Side.ToSideDto(),
				binanceOrder.Quantity,
				binanceOrder.Price,
				12345m);

		public static FuturesOrderDto ToFuturesOrderDto(this BinanceFuturesStreamOrderUpdateData streamOrder)
			=> new FuturesOrderDto(
				streamOrder.OrderId,
				streamOrder.Symbol,
				streamOrder.Side.ToSideDto(),
				streamOrder.BidNotional,
				streamOrder.Price,
				0);

		public static FuturesPositionDto ToFuturesPositionDto(this BinanceFuturesStreamPosition streamPosition, long id, OrderSide side)
		{
			return new FuturesPositionDto(
				id,
				streamPosition.Symbol,
				side.ToSideDto(),
				streamPosition.EntryPrice);
		}

		public static Core.Types.Side ToSideDto(this OrderSide orderSide)
			=> orderSide == OrderSide.Sell ? Core.Types.Side.Sell : Core.Types.Side.Buy;
	}
}
