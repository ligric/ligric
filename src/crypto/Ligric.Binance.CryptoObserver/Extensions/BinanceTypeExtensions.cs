using Binance.Net.Objects.Models.Futures;

namespace Ligric.CryptoObserver.Extensions
{
	public static class BinanceTypeExtensions
	{
		public static Binance.Types.BinanceFuturesFilledOrder ToBinanceFuturesFilledOrder(this BinanceFuturesOrder inputOrder)
			=> new Binance.Types.BinanceFuturesFilledOrder(inputOrder.Id,inputOrder.Symbol, inputOrder.Quantity,
				(decimal)inputOrder.QuoteQuantityFilled!, inputOrder.UpdateTime);
	}
}
