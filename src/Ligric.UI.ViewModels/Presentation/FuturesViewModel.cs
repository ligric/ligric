using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		public FuturesViewModel(
			IApiesService apiesService,
			IOrdersService orderService,
			IValuesService valuesService,
			IPositionsService postionsService)
		{
			Api = new ApisViewModel(apiesService, orderService, valuesService, postionsService);
			FuturesOrders = new FutureOrdersViewModel(orderService, valuesService);
			FuturePositions = new FuturePositionsViewModel(orderService, valuesService, postionsService);
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrders { get; }

		public FuturePositionsViewModel FuturePositions { get; }
	}
}
