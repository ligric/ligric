using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Ligric.Business.Futures;
using Ligric.Types.Future;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FutureOrdersViewModel
	{
		private readonly IDispatcher _dispatcher;
		private readonly IValuesService _valuesService;
		private readonly IOrdersService _ordersService;

		internal FutureOrdersViewModel(
			IDispatcher dispatcher,
			IOrdersService ordersService,
			IValuesService valuesService)
		{
			_dispatcher = dispatcher;
			_ordersService = ordersService;
			_valuesService = valuesService;

			_ordersService.OpenOrdersChanged += OnOpenOrdersChanged;
			_valuesService.ValuesChanged += OnValuesChanged;
		}

		public ObservableCollection<OrderViewModel> Orders { get; } = new ObservableCollection<OrderViewModel>();

		private void OnOpenOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> obj)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdateOrdersFromAction(obj);
			});
		}

		private void OnValuesChanged(object sender, NotifyDictionaryChangedEventArgs<string, decimal> obj)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdateOrdersFromAction(obj);
			});
		}

		private void UpdateOrdersFromAction(NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedOrder = obj.NewValue ?? throw new ArgumentException("Order is null");
					Orders.Add(addedOrder.ToOrderViewModel());
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedOrder = Orders.FirstOrDefault(x => x.Id == obj.Key.ToString());
					if (removedOrder == null) break;
					Orders.Remove(removedOrder);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedOrder = obj.NewValue ?? throw new ArgumentException("Order is null");
					var stringId = changedOrder.Id.ToString();
					for (int i = 0; i < Orders.Count; i++)
					{
						if (Orders[i].Id == stringId)
						{
							Orders[i] = changedOrder.ToOrderViewModel();
							break;
						}
					}
					break;
			}
		}

		private void UpdateOrdersFromAction(NotifyDictionaryChangedEventArgs<string, decimal> e)
		{
			if (e.Action == NotifyDictionaryChangedAction.Added
				|| e.Action == NotifyDictionaryChangedAction.Changed)
			{
				for (int i = 0; i < Orders.Count; i++)
				{
					if (Orders[i].Symbol == e.Key)
					{
						var oldValue = Orders[i];
						Orders[i] = new OrderViewModel
						{
							Id = oldValue.Id,
							Symbol = oldValue.Symbol,
							Order = oldValue.Order,
							Price = oldValue.Price,
							Quantity = oldValue.Quantity,
							Side = oldValue.Side,
							Value = e.NewValue.ToString()
						};
					}
				}
			}
		}
	}

}
