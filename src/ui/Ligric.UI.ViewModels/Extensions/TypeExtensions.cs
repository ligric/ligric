using Ligric.Core.Types.Future;
using Ligric.UI.ViewModels.Data;

namespace Ligric.UI.ViewModels.Extensions
{
	public static class TypeExtensions
	{
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
			return new PositionViewModel
			{
				Id = dto.Id.ToString(),
				ExchangeId = exchangeId,
				Symbol = dto.Symbol,
				OpenPrice = dto.EntryPrice.ToString(),
				CurrentPrice = "Nan",
				Side = dto.Side.ToString(),
				Quantity = dto.Quantity.ToString(),
				Leverage = dto.Leverage
			};
		}
	}
}
