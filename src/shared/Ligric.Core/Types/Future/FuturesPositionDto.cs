namespace Ligric.Core.Types.Future
{
	public record FuturesPositionDto
	{
		public long Id { get; init; }

		public string Symbol { get; init; }

		public PositionSide Side { get; init; }

		//public decimal Quantity { get; init; }

		public decimal EntryPrice { get; init; }

		//public decimal CurrentPrice { get; init; }

		//public decimal GrossPNL { get; init; }

		//public decimal GrossROE { get; init; }

		public FuturesPositionDto(long id, string symbol, PositionSide side, /*decimal quantity, */decimal openPrice/*, decimal currentPrice, decimal grossPNL, decimal grossROE*/)
		{
			Id = id;
			Symbol = symbol;
			Side = side;
			//Quantity = quantity;
			EntryPrice = openPrice;
			//CurrentPrice = currentPrice;
			//GrossPNL = grossPNL;
			//GrossROE = grossROE;
		}
	}
}
