using System.Collections;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Ligric.Business.Interfaces;
using Ligric.Business.Interfaces.Futures;
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
		private readonly IFuturesCryptoManager _futuresCryptoManager;

		internal FuturePositionsViewModel(
			IDispatcher dispatcher,
			IFuturesCryptoManager futuresCryptoManager)
		{
			_dispatcher = dispatcher;
			_futuresCryptoManager = futuresCryptoManager;

			_futuresCryptoManager.ClientsChanged += OnFuturesClientsChanged;
			_futuresCryptoManager.Clients.Values.ForEach(InitializePrimaryPositions);
		}

		public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

		private void InitializePrimaryPositions(IFuturesCryptoClient futuresClient)
		{
			futuresClient.ClientPositionsChanged += OnPositionsChanged;
			futuresClient.Trades.TradesChanged += OnTradesChanged;
			futuresClient.ClientLeveragesChanged += OnLeveragesChanged;

			lock (((ICollection)Positions).SyncRoot)
			{
				foreach (var position in futuresClient.Positions.Positions.Values)
				{
					var positionVm = position.ToPositionViewModel(futuresClient.ClientId);
					SetCurrentPrice(futuresClient, positionVm);
					SetLeverage(futuresClient, positionVm);
					Positions.Add(positionVm);
				}
			}
		}

		private void OnLeveragesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, IdentityEntity<LeverageDto>> e)
			=> _dispatcher.TryEnqueue(() => UpdatePostionsFromAction(e));

		private void OnPositionsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesPositionDto>> e)
			=> _dispatcher.TryEnqueue(() => UpdatePostionsFromAction(e));

		private void OnTradesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> e)
			=> _dispatcher.TryEnqueue(() => UpdatePostionsFromAction(e));

		private void UpdatePostionsFromAction(NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesPositionDto>> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedPosition = obj.NewValue?.Entity ?? throw new ArgumentException("Position is null");
					var client = GetClientFromClientId(obj.NewValue.Id)!;
					var positionVm = addedPosition.ToPositionViewModel(obj.NewValue.Id);
					SetCurrentPrice(client, positionVm);
					SetLeverage(client, positionVm);
					Positions.Add(positionVm);
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedPosition = Positions.FirstOrDefault(x => x.Id == obj.Key);
					if (removedPosition != null)
					{
						Positions.Remove(removedPosition);
					}
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedPosition = obj.NewValue?.Entity ?? throw new ArgumentException("Position is null");
					// _______________________
					//
					// TODO : need refactoring
					// _______________________
					for (int i = 0; i < Positions.Count; i++)
					{
						if (Positions[i].Id == changedPosition!.Id)
						{
							var changingPosition = Positions[i];
							changingPosition.EntryPrice = changedPosition.EntryPrice;
							changingPosition.Quantity = changedPosition.Quantity;
							changingPosition.QuoteQuantity = changedPosition.EntryPrice * changedPosition.Quantity;
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

		private void UpdatePostionsFromAction(NotifyDictionaryChangedEventArgs<string, IdentityEntity<LeverageDto>> obj)
		{
			switch(obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedLeverage = obj.NewValue?.Entity ?? throw new ArgumentException("Leverage is null");
					var client = GetClientFromClientId(obj.NewValue!.Id);
					for (int i = 0; i < Positions.Count; i++)
					{
						var position = Positions[i];
						if (position!.ClientId == obj.NewValue.Id
							&& position.Symbol == addedLeverage.Symbol)
						{
							var pnl = CalculatePnL(
									position.EntryPrice,
									position.CurrentPrice,
									(decimal)position.Quantity!);
							position.Size = CalculateSize(
								position.CurrentPrice,
								(decimal)position.Quantity!);
							position.Leverage = addedLeverage.Leverage;
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
				case NotifyDictionaryChangedAction.Changed:
					goto case NotifyDictionaryChangedAction.Added;
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

		private void OnFuturesClientsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, IFuturesCryptoClient> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					InitializePrimaryPositions(e.NewValue!);
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedClient = e.OldValue!;
					removedClient.ClientPositionsChanged -= OnPositionsChanged;
					removedClient.Trades.TradesChanged -= OnTradesChanged;
					removedClient.ClientLeveragesChanged -= OnLeveragesChanged;
					break;
				default: throw new NotImplementedException();
			}
		}

		private void SetCurrentPrice(IFuturesCryptoClient futuresClient, PositionViewModel positionVm)
		{
			if (futuresClient.Trades.Trades.TryGetValue(positionVm.Symbol!, out decimal value))
			{
				positionVm.CurrentPrice = value;
			}
		}

		private void SetLeverage(IFuturesCryptoClient futuresClient, PositionViewModel positionVm)
		{
			var leverage = futuresClient.Leverages.Leverages.Values.FirstOrDefault(x => x.Symbol == positionVm.Symbol!);
			if (leverage != null)
			{
				positionVm.Leverage = leverage.Leverage;
			}
		}

		private IFuturesCryptoClient? GetClientFromClientId(Guid clientId)
		{
			return _futuresCryptoManager.Clients.Values.FirstOrDefault(x => x.ClientId == clientId);
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
