using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Ligric.Business.Futures;
using Ligric.Domain.Types.Future;
using Ligric.UI.Infrastructure;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
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

			postionsCollectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnPositionsChanged);
#else
			_postionsService.PositionsChanged -= (s, e) => UpdatePostionsFromAction(e);
			_postionsService.PositionsChanged += (s, e) => UpdatePostionsFromAction(e);
#endif
		}

		private void OnPositionsChanged(NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> e)
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
	}
}
