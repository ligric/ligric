﻿namespace Ligric.Domain.Types.Future
{
    public record FuturesOrderDto
    {
		public long Id { get; init; }

		public string Symbol { get; init; }

		public Side Side { get; init; }

		public decimal Quantity { get; init; }

		public decimal Price { get; init; }

		public decimal Value { get; init; }

		public FuturesOrderDto(long id, string symbol, Side side, decimal quantity, decimal price, decimal value)
		{
			Id = id;
			Symbol = symbol;
			Side= side;
			Quantity= quantity;
			Price= price;
			Value= value;
		}
	}
}