using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Futures.Socket;
using Ligric.Domain.Types;
using Ligric.Domain.Types.Future;

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

		public static FuturesPositionDto ToFuturesPositionDto(this BinanceFuturesStreamPosition streamPosition)
		{
			return new FuturesPositionDto(0 /* 0 is temporary value */, streamPosition.Symbol, streamPosition.PositionSide.ToPositionSideDto(), streamPosition.EntryPrice);
		}

		public static Domain.Types.OrderSide ToOrderSideDto(this Binance.Net.Enums.OrderSide orderSide)
			=> orderSide == Binance.Net.Enums.OrderSide.Sell ? Domain.Types.OrderSide.Sell : Domain.Types.OrderSide.Buy;

		public static Domain.Types.PositionSide ToPositionSideDto(this Binance.Net.Enums.PositionSide positionSide)
		{
			if (positionSide == Binance.Net.Enums.PositionSide.Short) return Domain.Types.PositionSide.Short;
			if (positionSide == Binance.Net.Enums.PositionSide.Long) return Domain.Types.PositionSide.Long;
			return Domain.Types.PositionSide.Both;
		}
	}
}
