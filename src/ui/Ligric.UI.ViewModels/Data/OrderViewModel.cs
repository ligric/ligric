using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class OrderViewModel : ReactiveObject
	{
		public long Id { get; }
		public string Symbol { get; }
		public Guid ClientId { get; }

		public OrderViewModel(long id, Guid clientId, string symbol)
		{
			Id = id;
			ClientId = clientId;
			Symbol = symbol;
		}

		[Reactive] public string? Side { get; set; }
		[Reactive] public string? PositionSide { get; set; }
		[Reactive] public string? Quantity { get; set; }
		[Reactive] public string? Price { get; set; }
		[Reactive] public string? CurrentPrice { get; set; }
		[Reactive] public string? Type { get; set; }
		[Reactive] public string? StopPrice { get; set; }
	}
}
