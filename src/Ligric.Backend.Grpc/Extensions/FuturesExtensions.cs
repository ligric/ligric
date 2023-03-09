using Ligric.Types.Future;
using Ligric.Protos;

namespace Ligric.Backend.Grpc.Extensions
{
	public static class FuturesExtensions
	{
		public static FuturesOrder ToFutureOrder(this FuturesOrderDto dto)
		{
			return new FuturesOrder
			{
				Id = dto.Id,
				Price = dto.Price.ToString(),
				Quantity = dto.Quantity.ToString(),
				Side = dto.Side.ToOrderSideProto(),
				Symbol = dto.Symbol,
				Value = dto.Value.ToString(),
			};
		}

		public static Types.OrderSide ToOrderSideDto(this Protos.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case OrderSide.Sell:
					return Types.OrderSide.Sell;
				case OrderSide.Buy:
					return Types.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static OrderSide ToOrderSideProto(this Types.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case Types.OrderSide.Sell:
					return OrderSide.Sell;
				case Types.OrderSide.Buy:
					return OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Types.PositionSide ToPositionSideDto(this Protos.PositionSide sideInput)
		{
			switch (sideInput)	
			{
				case PositionSide.Short:
					return Types.PositionSide.Short;
				case PositionSide.Long:
					return Types.PositionSide.Long;
				case PositionSide.Both:
					return Types.PositionSide.Both;
			}
			throw new NotImplementedException();
		}

		public static PositionSide ToPositionSide(this Types.PositionSide sideInput)
		{
			switch (sideInput)	
			{
				case Types.PositionSide.Short:
					return PositionSide.Short;
				case Types.PositionSide.Long:
					return PositionSide.Long;
				case Types.PositionSide.Both:
					return PositionSide.Both;
			}
			throw new NotImplementedException();
		}

		public static FuturesPosition ToFuturesPosition(this FuturesPositionDto dto)
		{
			return new FuturesPosition
			{
				Id = dto.Id,
				Symbol = dto.Symbol,
				Side = dto.Side.ToPositionSide(),
				EntryPrice = dto.EntryPrice.ToString()
			};
		}
	}
}
