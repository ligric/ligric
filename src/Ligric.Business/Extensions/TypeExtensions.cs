using System;
using Ligric.Domain.Types.Api;
using Ligric.Domain.Types.Future;
using Ligric.Protos;

namespace Ligric.Business.Extensions
{
	internal static class TypeExtensions
	{
		public static ApiClientDto ToApiClientDto(this ApiClient apiClient)
		{
			return new ApiClientDto(apiClient.Id, apiClient.Name, apiClient.Permissions);
		}

		public static FuturesOrderDto ToFuturesOrderDto(this FuturesOrder futureOrder)
		{
			 return new FuturesOrderDto(
				 futureOrder.Id, futureOrder.Symbol, futureOrder.Side.ToOrderSideDto(),
				 decimal.Parse(futureOrder.Quantity), decimal.Parse(futureOrder.Price), decimal.Parse(futureOrder.Value));
		}

		public static FuturesPositionDto ToFuturesPositionDto(this FuturesPosition futuresPosition)
		{
			 return new FuturesPositionDto(
				 futuresPosition.Id, futuresPosition.Symbol, futuresPosition.Side.ToPositionSideDto(),
				 decimal.Parse(futuresPosition.EntryPrice));
		}

		public static Domain.Types.OrderSide ToOrderSideDto(this OrderSide sideInput)
		{
			switch (sideInput)
			{
				case OrderSide.Sell:
					return Domain.Types.OrderSide.Sell;
				case OrderSide.Buy:
					return Domain.Types.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Domain.Types.PositionSide ToPositionSideDto(this PositionSide sideInput)
		{
			switch (sideInput)
			{
				case PositionSide.Short:
					return Domain.Types.PositionSide.Short;
				case PositionSide.Long:
					return Domain.Types.PositionSide.Long;
				case PositionSide.Both:
					return Domain.Types.PositionSide.Both;
			}
			throw new NotImplementedException();
		}
	}
}
