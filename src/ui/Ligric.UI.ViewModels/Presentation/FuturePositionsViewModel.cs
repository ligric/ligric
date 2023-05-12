using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

		private void OnLeveragesChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdatePostionsFromAction(e);
			});
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
					var removedPosition = Positions.FirstOrDefault(x => x.Id == obj.Key);
					if (removedPosition == null) break;
					Positions.Remove(removedPosition);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedPosition = obj.NewValue?.Entity ?? throw new ArgumentException("Position is null");
					for (int i = 0; i < Positions.Count; i++)
					{
						if (Positions[i].Id == changedPosition.Id)
						{
							Positions[i] = changedPosition.ToPositionViewModel(obj.NewValue.ExchengedId);
							break;
						}
					}
					break;
			}
		}

		private void UpdatePostionsFromAction(NotifyCollectionChangedEventArgs obj)
		{
			switch (obj.Action)
			{
				case NotifyCollectionChangedAction.Add:
					var addedLeverage = (ExchangedEntity<LeverageDto>)(obj.NewItems?[0] ?? throw new ArgumentException("Leverage is null"));
					for (int i = 0; i < Positions.Count; i++)
					{
						var position = Positions[i];
						if (position.ExchangeId! == addedLeverage.ExchengedId
							&& position.Symbol == addedLeverage.Entity.Symbol)
						{
							Positions[i] = position with
							{
								Leverage = addedLeverage.Entity.Leverage,
								//PnL = CalculatePnL(position.EntryPrice, position.CurrentPrice, position.Quantity!),
								//PnLPercent = CalculateROE(position.EntryPrice, position.CurrentPrice)
							};
						}
					}
					break;

				case NotifyCollectionChangedAction.Replace:
					goto case NotifyCollectionChangedAction.Replace;
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
						var position = Positions[i];
						Positions[i] = position with
						{
							CurrentPrice = e.NewValue.ToString(),
							PnL = CalculatePnL(position.EntryPrice, e.NewValue, position.Quantity!),
							PnLPercent = CalculateROE(position.EntryPrice, e.NewValue)
						};
					}
				}
			}
		}

		private static decimal? CalculatePnL(decimal entryPrice, decimal currentPrice, string quantityUsdtString)
		{
			if (!decimal.TryParse(quantityUsdtString, out decimal quantityUsdt))
				return null;

			decimal pnl = (currentPrice - entryPrice) * quantityUsdt;
			return Math.Round(pnl, 2);
		}

		private static decimal? CalculateROE(decimal entryPriceString, decimal currentPrice)
		{
			decimal roe = (currentPrice - entryPriceString) / entryPriceString * 100;
			return Math.Round(roe, 2);
		}
	}
}
