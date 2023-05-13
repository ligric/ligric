using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Futures.Socket;

namespace Ligric.CryptoObserver.Extensions
{
	public static class BinanceTypeExtensions
	{
		public static Binance.Types.BinanceFuturesFilledOrder ToBinanceFuturesFilledOrder(this BinanceFuturesOrder inputOrder)
			=> new Binance.Types.BinanceFuturesFilledOrder(inputOrder.Id, inputOrder.Quantity, (decimal)inputOrder.QuoteQuantityFilled!, inputOrder.UpdateTime);

		//public static Binance.Types.BinanceFuturesFilledOrder ToBinanceFuturesFilledOrder(this BinanceFuturesStreamOrderUpdateData inputOrder)
		//	=> new Binance.Types.BinanceFuturesFilledOrder(inputOrder.OrderId, inputOrder.Quantity, (decimal)inputOrder.qu!, inputOrder.UpdateTime);
	}
}
