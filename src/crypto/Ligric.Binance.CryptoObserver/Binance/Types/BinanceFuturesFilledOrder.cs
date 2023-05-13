namespace Ligric.CryptoObserver.Binance.Types
{
	public record BinanceFuturesFilledOrder(long Id, decimal Quantity, decimal QuoteQuantity, DateTime UpdatedTime);
}
