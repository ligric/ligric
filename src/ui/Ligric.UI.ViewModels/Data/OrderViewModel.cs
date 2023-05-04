using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class OrderViewModel
	{
		[Reactive] public string? Id { get; set; }
		[Reactive] public string? Symbol { get; set; }
		[Reactive] public string? Side { get; set; }
		[Reactive] public string? Quantity { get; set; }
		[Reactive] public string? Price { get; set; }
		[Reactive] public string? Order { get; set; }
		[Reactive] public string? Value { get; set; }
		[Reactive] public string? Type { get; set; }
	}
}
