﻿using System.Collections.ObjectModel;
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
		private readonly IFuturesTradesService _valuesService;
		private readonly IPositionsService _postionsService;
		private readonly IFuturesLeveragesService _leverages;

		public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

		internal FuturePositionsViewModel(
			IDispatcher dispatcher,
			IFuturesTradesService valuesService,
			IPositionsService postionsService,
			IFuturesLeveragesService leveragesService)
		{
			_dispatcher = dispatcher;
			_valuesService = valuesService;
			_postionsService = postionsService;
			_leverages = leveragesService;

			_postionsService.PositionsChanged += OnPositionsChanged;
			_valuesService.TradesChanged += OnValuesChanged;
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
					if (addedPosition.Leverage == null)
					{
						var leverage = _leverages.Leverages.FirstOrDefault(
							x => x.ExchengedId == obj.NewValue.ExchengedId
								 && x.Entity.Symbol == addedPosition.Symbol);

						if (leverage != null)
						{
							addedPosition = addedPosition with { Leverage = leverage.Entity.Leverage };
						}
					}
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
							var changingPosition = Positions[i];
							changingPosition.EntryPrice = changedPosition.EntryPrice;
							changingPosition.Quantity = changedPosition.Quantity;
							changingPosition.QuoteQuantity = changedPosition.EntryPrice * changedPosition.Quantity;
							changingPosition.Leverage = changedPosition.Leverage;
							changingPosition.Size = CalculateSize(
								changingPosition.CurrentPrice,
								(decimal)changingPosition.Quantity!);
							changingPosition.PnL = CalculatePnL(
								changingPosition.EntryPrice,
								changingPosition.CurrentPrice,
								(decimal)changingPosition.Quantity!);
							changingPosition.PnLPercent = CalculateROE(
								changingPosition.EntryPrice,
								changingPosition.CurrentPrice,
								changingPosition.Leverage,
								(decimal)changingPosition.Quantity!,
								changingPosition.Side.Equals("Sell") ? (sbyte)-1 : (sbyte)1);
							break;
						}
					}
					break;
				case NotifyDictionaryChangedAction.Cleared:
					Positions.Clear();
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
							var pnl = CalculatePnL(
									position.EntryPrice,
									position.CurrentPrice,
									(decimal)position.Quantity!);
							position.Size = CalculateSize(
								position.CurrentPrice,
								(decimal)position.Quantity!);
							position.Leverage = addedLeverage.Entity.Leverage;
							position.PnL = pnl;
							position.PnLPercent = CalculateROE(
								position.EntryPrice,
								position.CurrentPrice,
								position.Leverage,
								(decimal)position.Quantity!,
								position.Side.Equals("Sell") ? (sbyte)-1 : (sbyte)1);
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
						var pnl = CalculatePnL(
								position.EntryPrice,
								e.NewValue,
								(decimal)position.Quantity!);
						position.CurrentPrice = e.NewValue;
						position.Size = CalculateSize(
							e.NewValue,
							(decimal)position.Quantity!);
						position.PnL = pnl;
						position.PnLPercent = CalculateROE(
							position.EntryPrice,
							e.NewValue,
							position.Leverage,
							(decimal)position.Quantity!,
							position.Side.Equals("Sell") ? (sbyte)-1 : (sbyte)1);
					}
				}
			}
		}

		private static decimal? CalculatePnL(decimal entryPrice, decimal? currentPrice, decimal quantityUsdt)
		{
			if (currentPrice == null) return null;

			decimal pnl = ((decimal)currentPrice - entryPrice) * quantityUsdt;
			return Math.Round(pnl, 2);
		}

		private static decimal? CalculateROE(decimal entryPrice, decimal? currentPrice, byte? leverage, decimal quantity, sbyte side)
		{
			if (currentPrice == null || leverage == null) return null;

			decimal imr = (decimal)1 / (byte)leverage;
			decimal roe = (side * (1 - entryPrice / (decimal)currentPrice) / imr) * 100;
			return Math.Round(roe, 2);
		}

		private static decimal? CalculateSize(decimal? currentPrice, decimal quantity)
		{
			if (currentPrice == null) return null;

			decimal size = (decimal)currentPrice * quantity;
			return Math.Round(size, 2);
		}
	}
}
