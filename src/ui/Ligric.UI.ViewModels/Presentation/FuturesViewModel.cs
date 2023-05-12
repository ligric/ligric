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
			IPositionsService postionsService,
			ILeveragesService leveragesService)
		{
			Api = new ApisViewModel(dispatcher, apiesService, orderService, valuesService, postionsService, leveragesService);
			FuturesOrders = new FutureOrdersViewModel(dispatcher, orderService, valuesService);
			FuturePositions = new FuturePositionsViewModel(dispatcher, valuesService, postionsService, leveragesService);
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrders { get; }

		public FuturePositionsViewModel FuturePositions { get; }
	}
}
