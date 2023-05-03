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
				binanceOrder.Side.ToOrderSideDto(),
				binanceOrder.Quantity,
				binanceOrder.Price,
				12345m);

		public static FuturesOrderDto ToFuturesOrderDto(this BinanceFuturesStreamOrderUpdateData streamOrder)
			=> new FuturesOrderDto(
				streamOrder.OrderId,
				streamOrder.Symbol,
				streamOrder.Side.ToOrderSideDto(),
				streamOrder.BidNotional,
				streamOrder.Price,
				0);

		public static FuturesPositionDto ToFuturesPositionDto(this BinanceFuturesStreamPosition streamPosition, long id)
		{
			return new FuturesPositionDto(
				id,
				streamPosition.Symbol,
				streamPosition.PositionSide.ToPositionSideDto(),
				streamPosition.EntryPrice);
		}

		public static Core.Types.OrderSide ToOrderSideDto(this OrderSide orderSide)
			=> orderSide == OrderSide.Sell ? Core.Types.OrderSide.Sell : Core.Types.OrderSide.Buy;

		public static Core.Types.PositionSide ToPositionSideDto(this PositionSide positionSide)
		{
			if (positionSide == PositionSide.Short) return Core.Types.PositionSide.Short;
			if (positionSide == Binance.Net.Enums.PositionSide.Long) return Core.Types.PositionSide.Long;
			return Core.Types.PositionSide.Both;
		}
	}
}
