using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		public FuturesViewModel(
			IDispatcher dispatcher,
			IApiesService apiesService,
			IOrdersService orderService,
			IValuesService valuesService,
			IPositionsService postionsService)
		{
			Api = new ApisViewModel(dispatcher, apiesService, orderService, valuesService, postionsService);
			FuturesOrders = new FutureOrdersViewModel(dispatcher, orderService, valuesService);
			FuturePositions = new FuturePositionsViewModel(dispatcher, orderService, valuesService, postionsService);
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrders { get; }

		public FuturePositionsViewModel FuturePositions { get; }
	}
}
