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

		private void OnOpenOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesOrderDto>> obj)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdateOrdersFromAction(obj);
			});
		}

		private void OnValuesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> obj)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdateOrdersFromAction(obj);
			});
		}

		private void UpdateOrdersFromAction(NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesOrderDto>> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedOrder = obj.NewValue?.Entity ?? throw new ArgumentException("Order is null");
					Orders.Add(addedOrder.ToOrderViewModel(obj.NewValue.ExchengedId));
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedOrder = Orders.FirstOrDefault(x => x.Id == obj.Key.ToString());
					if (removedOrder == null) break;
					Orders.Remove(removedOrder);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedOrder = obj.NewValue?.Entity ?? throw new ArgumentException("Order is null");
					var stringId = changedOrder.Id.ToString();
					for (int i = 0; i < Orders.Count; i++)
					{
						if (Orders[i].Id == stringId)
						{
							var changingItem = Orders[i];
							changingItem.CurrentPrice = changedOrder.CurrentPrice?.ToString();
							break;
						}
					}
					break;
				case NotifyDictionaryChangedAction.Cleared:
					Orders.Clear();
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
						var changingItem = Orders[i];
						changingItem.CurrentPrice = e.NewValue.ToString();
					}
				}
			}
		}
	}

}
