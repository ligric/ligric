using System.Collections;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Ligric.Business.Interfaces;
using Ligric.Business.Interfaces.Futures;
using Ligric.Core.Types.Future;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FutureOrdersViewModel
	{
		private readonly IDispatcher _dispatcher;
		private readonly IFuturesCryptoManager _futuresCryptoManager;

		internal FutureOrdersViewModel(
			IDispatcher dispatcher,
			IFuturesCryptoManager futuresCryptoManager)
		{
			_dispatcher = dispatcher;
			_futuresCryptoManager = futuresCryptoManager;

			_futuresCryptoManager.Clients.Values.ForEach(InitializePrimaryOrders);
		}

		public ObservableCollection<OrderViewModel> Orders { get; } = new ObservableCollection<OrderViewModel>();

		private void InitializePrimaryOrders(IFuturesCryptoClient futuresClient)
		{
			futuresClient.Orders.OrdersChanged += OnOrdersChanged;
			futuresClient.Trades.TradesChanged += OnTradesChanged;

			lock (((ICollection)Orders).SyncRoot)
			{
				foreach (var order in futuresClient.Orders.Orders.Values)
				{
					var orderVm = order.ToOrderViewModel();
					if (futuresClient.Trades.Trades.TryGetValue(orderVm.Symbol!, out decimal value))
					{
						orderVm.CurrentPrice = value.ToString();
					}
					Orders.Add(orderVm);
				}
			}
		}

		private void OnOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> obj)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdateOrdersFromAction(obj);
			});
		}

		private void OnTradesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> obj)
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
					if (removedOrder != null) Orders.Remove(removedOrder);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedOrder = obj.NewValue ?? throw new ArgumentException("Order is null");
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
