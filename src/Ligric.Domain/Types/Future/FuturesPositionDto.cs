namespace Ligric.Domain.Types.Future
{
	public record FuturesPositionDto
	{
		public long OrderId { get; init; }

		public string Symbol { get; init; }

		public Side Side { get; init; }

		public decimal Quantity { get; init; }

		public decimal OpenPrice { get; init; }

		public decimal CurrentPrice { get; init; }

		public decimal GrossPNL { get; init; }

		public decimal GrossROE { get; init; }

		public FuturesPositionDto(long orderId, string symbol, Side side, decimal quantity, decimal openPrice, decimal currentPrice, decimal grossPNL, decimal grossROE)
		{
			OrderId = orderId;
			Symbol = symbol;
			Side = side;
			Quantity = quantity;
			OpenPrice = openPrice;
			CurrentPrice = currentPrice;
			GrossPNL = grossPNL;
			GrossROE = grossROE;
		}
	}
}
