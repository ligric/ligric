using Ligric.Domain.Types;
using Ligric.Domain.Types.Future;
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

		public static Ligric.Domain.Types.OrderSide ToOrderSideDto(this Protos.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case Protos.OrderSide.Sell:
					return Ligric.Domain.Types.OrderSide.Sell;
				case Protos.OrderSide.Buy:
					return Ligric.Domain.Types.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Protos.OrderSide ToOrderSideProto(this Ligric.Domain.Types.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Domain.Types.OrderSide.Sell:
					return Protos.OrderSide.Sell;
				case Ligric.Domain.Types.OrderSide.Buy:
					return Protos.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Ligric.Domain.Types.PositionSide ToPositionSideDto(this Protos.PositionSide sideInput)
		{
			switch (sideInput)	
			{
				case Protos.PositionSide.Short:
					return Ligric.Domain.Types.PositionSide.Short;
				case Protos.PositionSide.Long:
					return Ligric.Domain.Types.PositionSide.Long;
				case Protos.PositionSide.Both:
					return Ligric.Domain.Types.PositionSide.Both;
			}
			throw new NotImplementedException();
		}

		public static Protos.PositionSide ToPositionSide(this Ligric.Domain.Types.PositionSide sideInput)
		{
			switch (sideInput)	
			{
				case Ligric.Domain.Types.PositionSide.Short:
					return Protos.PositionSide.Short;
				case Ligric.Domain.Types.PositionSide.Long:
					return Protos.PositionSide.Long;
				case Ligric.Domain.Types.PositionSide.Both:
					return Protos.PositionSide.Both;
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
