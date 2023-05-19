using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class PositionViewModel : ReactiveObject
	{
		public PositionViewModel(long id, Guid clientId, string symbol, string side, decimal entryPrice)
		{
			Id = id;
			Symbol = symbol;
			Side = side;
			EntryPrice = entryPrice;
		}
		public long Id { get; }
		public Guid ClientId { get; } 
		public string Symbol { get; }
		public string Side { get; }

		[Reactive] public decimal EntryPrice { get; set; }
		[Reactive] public decimal? CurrentPrice { get; set; }
		[Reactive] public decimal? Quantity { get; set; }
		[Reactive] public decimal? QuoteQuantity { get; set; }
		[Reactive] public decimal? Size { get; set; }
		[Reactive] public decimal? PnL { get; set; }
		[Reactive] public decimal? PnLPercent { get; set; }
		[Reactive] public byte? Leverage { get; set; }
	}
}
