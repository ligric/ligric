using System.ComponentModel;
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
				binanceOrder.PositionSide.ToSideDto(),
				binanceOrder.Quantity,
				binanceOrder.Price,
				null,
				binanceOrder.Type.ToOrderTypeDto());

		public static FuturesOrderDto ToFuturesOrderDto(this BinanceFuturesStreamOrderUpdateData streamOrder)
			=> new FuturesOrderDto(
				streamOrder.OrderId,
				streamOrder.Symbol,
				streamOrder.PositionSide.ToSideDto(),
				streamOrder.BidNotional,
				streamOrder.Price,
				null,
				streamOrder.Type.ToOrderTypeDto());

		public static FuturesPositionDto ToFuturesPositionDto(this BinanceFuturesStreamPosition streamPosition, long id, OrderSide side, byte? leverage)
		{
			return new FuturesPositionDto(
				id,
				streamPosition.Symbol,
				side.ToSideDto(),
				streamPosition.Quantity,
				streamPosition.EntryPrice,
				leverage);
		}

		public static FuturesPositionDto ToFuturesPositionDto(this BinancePositionDetailsUsdt binancePosition, long id, OrderSide side, byte? leverage, decimal quantityUsdt)
		{
			return new FuturesPositionDto(
				id,
				binancePosition.Symbol,
				side.ToSideDto(),
				quantityUsdt,
				binancePosition.EntryPrice,
				leverage);
		}

		public static Core.Types.Side ToSideDto(this OrderSide orderSide)
			=> orderSide == OrderSide.Sell ? Core.Types.Side.Sell : Core.Types.Side.Buy;

		public static Core.Types.Side ToSideDto(this PositionSide positionSide)
			=> positionSide switch
			{
				PositionSide.Short => Core.Types.Side.Sell,
				PositionSide.Long => Core.Types.Side.Buy,
				_ => throw new InvalidEnumArgumentException(nameof(positionSide), (int)positionSide, typeof(PositionSide))
			};

		public static Core.Types.OrderType ToOrderTypeDto(this FuturesOrderType type)
			=> type switch
			{
				FuturesOrderType.Limit => Core.Types.OrderType.Limit,
				FuturesOrderType.Liquidation => Core.Types.OrderType.Liquidation,
				FuturesOrderType.TrailingStopMarket => Core.Types.OrderType.TrailingStopMarket,
				FuturesOrderType.StopMarket => Core.Types.OrderType.StopMarket,
				FuturesOrderType.Market => Core.Types.OrderType.Market,
				FuturesOrderType.TakeProfitMarket => Core.Types.OrderType.TakeProfitMarket,
				FuturesOrderType.TakeProfit => Core.Types.OrderType.TakeProfit,
				FuturesOrderType.Stop => Core.Types.OrderType.Stop,
				_ => throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(FuturesOrderType))
			};
	}
}
