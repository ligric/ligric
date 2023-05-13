namespace Ligric.CryptoObserver.Binance.Types
{
	public record BinanceFuturesFilledOrder(long Id, string Symbol, decimal Quantity, decimal QuoteQuantity, DateTime UpdatedTime);
}
