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
				binanceOrder.Side.ToSide(),
				binanceOrder.Quantity,
				binanceOrder.Price,
				12345m);

		public static FuturesOrderDto ToFuturesOrderDto(this BinanceFuturesStreamOrderUpdateData streamOrder)
			=> new FuturesOrderDto(
				streamOrder.OrderId,
				streamOrder.Symbol,
				streamOrder.Side.ToSide(),
				streamOrder.Quantity,
				streamOrder.Price,
				12345m);

		public static Side ToSide(this OrderSide orderSide)
			=> orderSide == OrderSide.Sell ? Side.Sell : Side.Buy;
	}
}
