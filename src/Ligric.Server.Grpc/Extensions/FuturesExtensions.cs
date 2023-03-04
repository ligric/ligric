using Ligric.Domain.Types.Future;
using Ligric.Protos;
using Utils;

namespace Ligric.Server.Grpc.Extensions
{
	public static class FuturesExtensions
	{
		public static FutureOrder ToFutureOrder(this FuturesOrderDto dto)
		{
			return new FutureOrder
			{
				Id = dto.Id,
				Price = dto.Price.ToString(),
				Quantity = dto.Quantity.ToString(),
				Side = dto.Side.ToSideProto(),
				Symbol = dto.Symbol,
				Value = dto.Value.ToString(),
			};
		}

		public static Ligric.Domain.Types.Side ToSideDto(this Side sideInput)
		{
			switch (sideInput)
			{
				case Side.Sell:
					return Ligric.Domain.Types.Side.Sell;
				case Side.Buy:
					return Ligric.Domain.Types.Side.Buy;
			}
			throw new NotImplementedException();
		}

		public static Side ToSideProto(this Ligric.Domain.Types.Side sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Domain.Types.Side.Sell:
					return Side.Sell;
				case Ligric.Domain.Types.Side.Buy:
					return Side.Buy;
			}
			throw new NotImplementedException();
		}
	}
}
