using System;
using Ligric.Domain.Types.Api;
using Ligric.Protos;

namespace Ligric.Business.Extensions
{
	internal static class TypeExtensions
	{
		public static ApiClientDto ToApiClientDto(this ApiClient apiClient)
		{
			return new ApiClientDto(apiClient.Id, apiClient.Name, apiClient.Permissions);
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
