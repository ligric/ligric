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

			_futuresCryptoManager.ClientsChanged += OnFuturesClientsChanged;
			_futuresCryptoManager.Clients.Values.ForEach(InitializePrimaryOrders);
		}

		public ObservableCollection<OrderViewModel> Orders { get; } = new ObservableCollection<OrderViewModel>();

		private void InitializePrimaryOrders(IFuturesCryptoClient futuresClient)
		{
			futuresClient.ClientOrdersChanged += OnOrdersChanged;
			futuresClient.Trades.TradesChanged += OnTradesChanged;

			lock (((ICollection)Orders).SyncRoot)
			{
				foreach (var order in futuresClient.Orders.Orders.Values)
				{
					var orderVm = order.ToOrderViewModel(futuresClient.ClientId);
					SetCurrentPrice(futuresClient, orderVm);
					Orders.Add(orderVm);
				}
			}
		}

		private void OnOrdersChanged(object? sender, NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesOrderDto>> obj)
			=> _dispatcher.TryEnqueue(() => UpdateOrdersFromAction(obj));

		private void OnTradesChanged(object? sender, NotifyDictionaryChangedEventArgs<string, decimal> obj)
			=> _dispatcher.TryEnqueue(() => UpdateOrdersFromAction(obj));

		private void UpdateOrdersFromAction(NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesOrderDto>> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedOrder = obj.NewValue?.Entity ?? throw new ArgumentException("Order is null");
					if (Orders.FirstOrDefault(x => x.Id == addedOrder.Id) != null) break;

					var orderVm = addedOrder.ToOrderViewModel(obj.NewValue.Id!);
					var client = GetClientFromClientId(obj.NewValue.Id!)!;
					SetCurrentPrice(client, orderVm);
					Orders.Add(orderVm);
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedOrder = Orders.FirstOrDefault(x => x.Id == obj.Key);
					if (removedOrder != null)
					{
						Orders.Remove(removedOrder);
					}
					break;
				case NotifyDictionaryChangedAction.Changed:
					throw new NotImplementedException();
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

		private void OnFuturesClientsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, IFuturesCryptoClient> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					InitializePrimaryOrders(e.NewValue!);
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedClient = e.OldValue!;
					removedClient.ClientOrdersChanged -= OnOrdersChanged;
					removedClient.Trades.TradesChanged -= OnTradesChanged;
					break;
				case NotifyDictionaryChangedAction.Cleared:
					// TODO : Not implemented.
					break;
				default: throw new NotImplementedException();
			}
		}

		private void SetCurrentPrice(IFuturesCryptoClient futuresClient, OrderViewModel orderVm)
		{
			if (futuresClient.Trades.Trades.TryGetValue(orderVm.Symbol!, out decimal value))
			{
				orderVm.CurrentPrice = value.ToString();
			}
		}

		private IFuturesCryptoClient? GetClientFromClientId(Guid clientId)
		{
			return _futuresCryptoManager.Clients.Values.FirstOrDefault(x => x.ClientId == clientId);
		}
	}
}
