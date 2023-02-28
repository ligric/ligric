using System.Collections.ObjectModel;
using Ligric.Business.Futures;
using Ligric.UI.ViewModels.Data;
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

		private void OnOpenOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, Domain.Types.Future.FuturesOrderDto> e) => throw new NotImplementedException();
	}

}
