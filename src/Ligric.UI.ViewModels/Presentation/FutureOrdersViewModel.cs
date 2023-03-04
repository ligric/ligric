using System.Collections.ObjectModel;
using Ligric.Business.Futures;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FutureOrdersViewModel
	{
		private readonly IOrdersService _ordersService;
		public FutureOrdersViewModel(IOrdersService ordersService)
		{
			_ordersService = ordersService;

			_ordersService.OpenOrdersChanged += OnOpenOrdersChanged;
		}

		public ObservableCollection<OrderViewModel> Orders { get; } = new ObservableCollection<OrderViewModel>();

		private void OnOpenOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, Domain.Types.Future.FuturesOrderDto> e)
		{
			switch (e.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var newOrder = e.NewValue ?? throw new ArgumentException("Order is null");
					Orders.Add(newOrder.ToOrderViewModel());
					break;
				case NotifyDictionaryChangedAction.Removed:
					break;
				case NotifyDictionaryChangedAction.Changed:
					break;
			}
		}
	}

}
