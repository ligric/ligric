using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public partial record PositionViewModel(long Id, Guid ExchangeId, string Symbol, string Side, decimal EntryPrice)
	{
		[Reactive] public decimal? CurrentPrice { get; set; }
		[Reactive] public decimal? Quantity { get; set; }
		[Reactive] public decimal? PnL { get; set; }
		[Reactive] public decimal? PnLPercent { get; set; }
		[Reactive] public byte? Leverage { get; set; }
	}
}
