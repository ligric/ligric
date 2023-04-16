using System;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;

namespace Ligric.Business.Extensions
{
	internal static class TypeExtensions
	{
		public static ApiClientDto ToApiClientDto(this ApiClient apiClient)
		{
			return new ApiClientDto(apiClient.Id, apiClient.Name, apiClient.Permissions);
		}

		//public static FuturesOrderDto ToFuturesOrderDto(this FuturesOrder futureOrder)
		//{
		//	 return new FuturesOrderDto(
		//		 futureOrder.Id, futureOrder.Symbol, futureOrder.Side.ToOrderSideDto(),
		//		 decimal.Parse(futureOrder.Quantity), decimal.Parse(futureOrder.Price), decimal.Parse(futureOrder.Value));
		//}

		//public static FuturesPositionDto ToFuturesPositionDto(this FuturesPosition futuresPosition)
		//{
		//	 return new FuturesPositionDto(
		//		 futuresPosition.Id, futuresPosition.Symbol, futuresPosition.Side.ToPositionSideDto(),
		//		 decimal.Parse(futuresPosition.EntryPrice));
		//}

		//public static Core.Types.OrderSide ToOrderSideDto(this OrderSide sideInput)
		//{
		//	switch (sideInput)
		//	{
		//		case OrderSide.Sell:
		//			return Core.Types.OrderSide.Sell;
		//		case OrderSide.Buy:
		//			return Core.Types.OrderSide.Buy;
		//	}
		//	throw new NotImplementedException();
		//}

		//public static Core.Types.PositionSide ToPositionSideDto(this PositionSide sideInput)
		//{
		//	switch (sideInput)
		//	{
		//		case PositionSide.Short:
		//			return Core.Types.PositionSide.Short;
		//		case PositionSide.Long:
		//			return Core.Types.PositionSide.Long;
		//		case PositionSide.Both:
		//			return Core.Types.PositionSide.Both;
		//	}
		//	throw new NotImplementedException();
		//}
	}
}
