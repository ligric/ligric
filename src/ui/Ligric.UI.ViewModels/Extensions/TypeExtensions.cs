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

		public static OrderViewModel ToOrderViewModel(this FuturesOrderDto dto, Guid exchangeId)
		{
			return new OrderViewModel
			{
				Id = dto.Id.ToString(),
				ExchangeId = exchangeId,
				Symbol = dto.Symbol,
				Price = dto.Price.ToString(),
				Side = dto.Side.ToString(),
				Quantity = dto.Quantity.ToString(),
				CurrentPrice = dto.CurrentPrice.ToString(),
				Type = dto.Type.ToString(),
			};
		}

		public static PositionViewModel ToPositionViewModel(this FuturesPositionDto dto, Guid exchangeId)
		{
			return new PositionViewModel(dto.Id, exchangeId, dto.Symbol, dto.Side.ToString(), dto.EntryPrice)
			{
				Quantity = dto.Quantity,
				QuoteQuantity = dto.EntryPrice * dto.Quantity,
				Leverage = dto.Leverage,
			};
		}
	}
}
