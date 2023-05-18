using System.Reactive;
using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Futures;
using ReactiveUI;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		private readonly IAuthorizationService _authorizationService;
		public FuturesViewModel(
			IDispatcher dispatcher,
			IApiesService apiesService,
			IFuturesOrdersService orderService,
			IFuturesTradesService valuesService,
			IFuturesPositionsService postionsService,
			IFuturesLeveragesService leveragesService,
			IAuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
			Api = new ApisViewModel(dispatcher, apiesService, orderService, valuesService, postionsService, leveragesService);
			FuturesOrders = new FutureOrdersViewModel(dispatcher, orderService, valuesService);
			FuturePositions = new FuturePositionsViewModel(dispatcher, valuesService, postionsService, leveragesService);
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrders { get; }

		public FuturePositionsViewModel FuturePositions { get; }

		public ReactiveCommand<Unit, Unit> LogoutCommand => ReactiveCommand.Create(Logout);

		private void Logout()
		{
			_authorizationService.Logout();
		}
	}
}
