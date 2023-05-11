using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Ligric.Business.Futures;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturePositionsViewModel
	{
		private readonly IDispatcher _dispatcher;
		private readonly IValuesService _valuesService;
		private readonly IPositionsService _postionsService;
		private readonly ILeveragesService _leverages;

		public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

		internal FuturePositionsViewModel(
			IDispatcher dispatcher,
			IValuesService valuesService,
			IPositionsService postionsService,
			ILeveragesService leveragesService)
		{
			_dispatcher = dispatcher;
			_valuesService = valuesService;
			_postionsService = postionsService;
			_leverages = leveragesService;

			_postionsService.PositionsChanged += OnPositionsChanged;
			_valuesService.ValuesChanged += OnValuesChanged;
			_leverages.LeveragesChanged += OnLeveragesChanged;
		}

		private void OnLeveragesChanged(object? sender, NotifyDictionaryChangedEventArgs<Guid, LeverageDto> e)
		{
			//switch (e.Key)
			//{

			//}
		}

		private void OnPositionsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesPositionDto>> e)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdatePostionsFromAction(e);
			});
		}

		private void OnValuesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> e)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdatePostionsFromAction(e);
			});
		}

		private void UpdatePostionsFromAction(NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesPositionDto>> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedPosition = obj.NewValue?.Entity ?? throw new ArgumentException("Position is null");
					Positions.Add(addedPosition.ToPositionViewModel(obj.NewValue.ExchengedId));
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedPosition = Positions.FirstOrDefault(x => x.Id == obj.Key.ToString());
					if (removedPosition == null) break;
					Positions.Remove(removedPosition);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedPosition = obj.NewValue?.Entity ?? throw new ArgumentException("Position is null");
					var stringId = changedPosition.Id.ToString();
					for (int i = 0; i < Positions.Count; i++)
					{
						if (Positions[i].Id == stringId)
						{
							Positions[i] = changedPosition.ToPositionViewModel(obj.NewValue.ExchengedId);
							break;
						}
					}
					break;
			}
		}

		private void UpdatePostionsFromAction(NotifyDictionaryChangedEventArgs<string, decimal> e)
		{
			if (e.Action == NotifyDictionaryChangedAction.Added
				|| e.Action == NotifyDictionaryChangedAction.Changed)
			{
				for (int i = 0; i < Positions.Count; i++)
				{
					if (Positions[i].Symbol == e.Key)
					{
						var oldValue = Positions[i];
						Positions[i] = new PositionViewModel
						{
							Id = oldValue.Id,
							Symbol = oldValue.Symbol,
							Side = oldValue.Side,
							Quantity = oldValue.Quantity,
							OpenPrice = oldValue.OpenPrice,
							CurrentPrice = e.NewValue.ToString(),
							PnL = CalculatePnL(oldValue.OpenPrice!, e.NewValue, oldValue.Quantity!),
							PnLPercent = CalculateROE(oldValue.OpenPrice!, e.NewValue)
						};
					}
				}
			}
		}

		private static decimal? CalculatePnL(string openPriceString, decimal currentPrice, string quantityUsdtString)
		{
			if (!decimal.TryParse(openPriceString, out decimal openPrice)
				|| !decimal.TryParse(quantityUsdtString, out decimal quantityUsdt))
				return null;

			decimal pnl = (currentPrice - openPrice) * quantityUsdt;
			return Math.Round(pnl, 2);
		}

		private static decimal? CalculateROE(string openPriceString, decimal currentPrice)
		{
			if (!decimal.TryParse(openPriceString, out decimal openPrice))
				return null;

			decimal roe = (currentPrice - openPrice) / openPrice * 100;
			return Math.Round(roe, 2);
		}
	}
}
