using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Ligric.Business.Futures;
using Ligric.Domain.Types.Future;
using Ligric.Protos;
using Ligric.UI.Infrastructure;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using Microsoft.UI.Xaml.Controls;
using ReactiveUI.Fody.Helpers;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturePositionsViewModel
	{
		private readonly IOrdersService _ordersService;
		private readonly IValuesService _valuesService;
		private readonly IPositionsService _postionsService;

		public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

		internal FuturePositionsViewModel(
			IOrdersService ordersService,
			IValuesService valuesService,
			IPositionsService postionsService)
		{
			_ordersService = ordersService;
			_valuesService = valuesService;
			_postionsService = postionsService;

			Subscribtions();
		}

		private void Subscribtions()
		{
#if !__WASM__
			var postionsCollectionChanged = Observable.FromEvent<EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>(handler =>
			{
				EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>> collectionChanged = (sender, e) =>
				{
					handler(e);
				};

				return collectionChanged;
			}, fsHandler => _postionsService.PositionsChanged += fsHandler, fsHandler => _postionsService.PositionsChanged -= fsHandler);

			var valuesCollectionChanged = Observable.FromEvent<EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>, NotifyDictionaryChangedEventArgs<string, decimal>>(handler =>
			{
				EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>> collectionChanged = (sender, e) =>
				{
					handler(e);
				};

				return collectionChanged;
			}, fsHandler => _valuesService.ValuesChanged += fsHandler, fsHandler => _valuesService.ValuesChanged -= fsHandler);

			postionsCollectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnPositionsChanged);
			valuesCollectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnValuesChanged);

#else
			_postionsService.PositionsChanged -= (s, e) => UpdatePostionsFromAction(e);
			_postionsService.PositionsChanged += (s, e) => UpdatePostionsFromAction(e);
			
			_valuesService.ValuesChanged -= (s, e) => OnValuesChanged(e);
			_valuesService.ValuesChanged += (s, e) => OnValuesChanged(e);
#endif
		}

		private void OnPositionsChanged(NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> e)
		{
			UpdatePostionsFromAction(e);
		}

		private void OnValuesChanged(NotifyDictionaryChangedEventArgs<string, decimal> e)
		{
			UpdatePostionsFromAction(e);
		}

		private void UpdatePostionsFromAction(NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedPosition = obj.NewValue ?? throw new ArgumentException("Order is null");
					Positions.Add(addedPosition.ToPositionViewModel());
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedPosition = Positions.FirstOrDefault(x => x.Id == obj.Key.ToString());
					if (removedPosition == null) break;
					Positions.Remove(removedPosition);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedPosition = obj.NewValue ?? throw new ArgumentException("Order is null");
					var stringId = changedPosition.Id.ToString();
					for (int i = 0; i < Positions.Count; i++)
					{
						if (Positions[i].Id == stringId)
						{
							Positions[i] = changedPosition.ToPositionViewModel();
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
							PnL = oldValue.PnL,
							PnLPercent = oldValue.PnLPercent,
						};
					}
				}
			}
		}
	}
}
