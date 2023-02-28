using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		public FuturesViewModel(
			IApiesService apiesService,
			IOrdersService orderService)
		{
			Api = new ApisViewModel(apiesService, orderService);
			FuturesOrders = new FutureOrdersViewModel(orderService);
			FuturePositions = new FuturePositionsViewModel();
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrders { get; }

		public FuturePositionsViewModel FuturePositions { get; }
	}
}
