using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class PositionViewModel
	{
		[Reactive] public string? Symbol { get; set; }
		[Reactive] public string? Side { get; set; }
		[Reactive] public string? Quantity { get; set; }
		[Reactive] public string? OpenPrice { get; set; }
		[Reactive] public string? CurrentPrice { get; set; }
		[Reactive] public string? PnL { get; set; }
		[Reactive] public string? PnLPercent { get; set; }
	}
}
