﻿using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public partial record PositionViewModel(long Id, Guid ExchangeId, string Symbol, string Side, decimal EntryPrice)
	{
		[Reactive] public string? CurrentPrice { get; set; }
		[Reactive] public string? Quantity { get; set; }
		[Reactive] public decimal? PnL { get; set; }
		[Reactive] public decimal? PnLPercent { get; set; }
		[Reactive] public byte? Leverage { get; set; }
	}
}
