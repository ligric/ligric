using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		public FuturesViewModel(
			IApiesService apiesService,
			IOrdersService orderService,
			IValuesService valuesService)
		{
			Api = new ApisViewModel(apiesService, orderService, valuesService);
			FuturesOrders = new FutureOrdersViewModel(orderService, valuesService);
			FuturePositions = new FuturePositionsViewModel(orderService, valuesService);
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrders { get; }

		public FuturePositionsViewModel FuturePositions { get; }
	}
}
