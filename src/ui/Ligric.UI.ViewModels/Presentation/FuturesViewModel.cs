using System.Reactive;
using Ligric.Business.Apies;
using Ligric.Business.Interfaces;
using Ligric.Business.Interfaces.Futures;
using ReactiveUI;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		private readonly IAuthorizationService _authorizationService;

		public FuturesViewModel(
			IDispatcher dispatcher,
			IApiesService apiesService,
			IFuturesCryptoManager futuresCryptoManager,
			IAuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
			Api = new ApisViewModel(dispatcher, apiesService, futuresCryptoManager);
			FuturesOrders = new FutureOrdersViewModel(dispatcher, futuresCryptoManager);
			FuturePositions = new FuturePositionsViewModel(dispatcher, futuresCryptoManager);
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
