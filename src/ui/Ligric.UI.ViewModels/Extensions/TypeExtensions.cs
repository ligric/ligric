using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types.Future;
using Ligric.UI.ViewModels.Data;

namespace Ligric.UI.ViewModels.Extensions
{
	public static class TypeExtensions
	{
		public static ApiClientViewModel ToApiClientViewModel(this ApiClientDto dto)
		{
			return new ApiClientViewModel
			{
				UserApiId = dto.UserApiId,
				Name = dto.Name,
				Permissions = dto.Permissions
			};
		}

		public static ApiClientDto ToApiClientDto(this ApiClientViewModel vm)
		{
			return new ApiClientDto(vm.UserApiId, vm.Name!, vm.Permissions ?? throw new ArgumentNullException("[ApiClientViewModel] Permissions is null."));
		}

		public static OrderViewModel ToOrderViewModel(this FuturesOrderDto dto, Guid clientId)
		{
			return new OrderViewModel(dto.Id, clientId, dto.Symbol)
			{
				Price = dto.Price.ToString(),
				Side = dto.Side.ToString(),
				PositionSide = dto.PositionSide.ToString(),
				Quantity = dto.Quantity.ToString(),
				CurrentPrice = dto.CurrentPrice.ToString(),
				Type = dto.Type.ToString(),
				StopPrice = dto.StopPrice.ToString()
			};
		}

		public static PositionViewModel ToPositionViewModel(this FuturesPositionDto dto, Guid clientId)
		{
			return new PositionViewModel(dto.Id, clientId, dto.Symbol, dto.Side.ToString(), dto.EntryPrice)
			{
				Quantity = dto.Quantity,
				QuoteQuantity = dto.EntryPrice * dto.Quantity,
				Leverage = dto.Leverage,
			};
		}
	}
}
