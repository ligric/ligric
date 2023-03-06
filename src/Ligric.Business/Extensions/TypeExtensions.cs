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
				 futureOrder.Id, futureOrder.Symbol, futureOrder.Side.ToSideDto(),
				 decimal.Parse(futureOrder.Quantity), decimal.Parse(futureOrder.Price), decimal.Parse(futureOrder.Value));
		}

		public static Domain.Types.OrderSide ToSideDto(this OrderSide sideInput)
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
	}
}
