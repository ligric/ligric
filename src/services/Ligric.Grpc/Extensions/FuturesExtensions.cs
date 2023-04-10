using Ligric.Protobuf;
using Ligric.Core.Types.Future;

namespace Ligric.Grpc.Extensions
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

		public static Core.Types.OrderSide ToOrderSideDto(this Protos.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case OrderSide.Sell:
					return Core.Types.OrderSide.Sell;
				case OrderSide.Buy:
					return Core.Types.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static OrderSide ToOrderSideProto(this Core.Types.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case Core.Types.OrderSide.Sell:
					return OrderSide.Sell;
				case Core.Types.OrderSide.Buy:
					return OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Core.Types.PositionSide ToPositionSideDto(this Protos.PositionSide sideInput)
		{
			switch (sideInput)	
			{
				case PositionSide.Short:
					return Core.Types.PositionSide.Short;
				case PositionSide.Long:
					return Core.Types.PositionSide.Long;
				case PositionSide.Both:
					return Core.Types.PositionSide.Both;
			}
			throw new NotImplementedException();
		}

		public static PositionSide ToPositionSide(this Core.Types.PositionSide sideInput)
		{
			switch (sideInput)	
			{
				case Core.Types.PositionSide.Short:
					return PositionSide.Short;
				case Core.Types.PositionSide.Long:
					return PositionSide.Long;
				case Core.Types.PositionSide.Both:
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
