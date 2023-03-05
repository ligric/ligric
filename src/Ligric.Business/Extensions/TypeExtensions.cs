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

		public static FuturesOrderDto ToFuturesOrderDto(this FutureOrder futureOrder)
		{
			 return new FuturesOrderDto(
				 futureOrder.Id, futureOrder.Symbol, futureOrder.Side.ToSideDto(),
				 decimal.Parse(futureOrder.Quantity), decimal.Parse(futureOrder.Price), decimal.Parse(futureOrder.Value));
		}

		public static Domain.Types.Side ToSideDto(this Side sideInput)
		{
			switch (sideInput)
			{
				case Side.Sell:
					return Domain.Types.Side.Sell;
				case Side.Buy:
					return Domain.Types.Side.Buy;
			}
			throw new NotImplementedException();
		}
	}
}
