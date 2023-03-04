using Ligric.Domain.Types.Future;
using Ligric.UI.ViewModels.Data;

namespace Ligric.UI.ViewModels.Extensions
{
	public static class TypeExtensions
	{
		public static OrderViewModel ToOrderViewModel(this FuturesOrderDto dto)
		{
			return new OrderViewModel
			{
				Id = dto.Id.ToString(),
				Symbol = dto.Symbol,
				Price = dto.Price.ToString(),
				Side = dto.Side.ToString(),
				Quantity = dto.Quantity.ToString(),
				Value = dto.Value.ToString(),
				Order = "???"
			};
		}
	}
}
