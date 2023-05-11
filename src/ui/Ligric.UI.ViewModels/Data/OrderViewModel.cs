using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class OrderViewModel
	{
		[Reactive] public string? Id { get; set; }
		[Reactive] public Guid? ExchangeId { get; set; }
		[Reactive] public string? Symbol { get; set; }
		[Reactive] public string? Side { get; set; }
		[Reactive] public string? Quantity { get; set; }
		[Reactive] public string? Price { get; set; }
		[Reactive] public string? CurrentPrice { get; set; }
		[Reactive] public string? Type { get; set; }
	}
}
