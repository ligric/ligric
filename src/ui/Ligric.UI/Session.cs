using Ligric.Business.Interfaces;

namespace Ligric.UI
{
	internal static class Session
	{
		private static IAuthorizationService? _authorizationService;
		private static IServiceCollection? _clientServices;

		public static void SetSession(
			IAuthorizationService authorizationService,
			IServiceCollection clientServices)
		{
			_authorizationService = authorizationService;
			_clientServices = clientServices;

			_authorizationService.AuthorizationStateChanged -= OnAuthorizationStateChanged;
			_authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
		}

		private static void OnAuthorizationStateChanged(object? sender, Core.Types.User.UserAuthorizationState e)
		{
			switch (e)
			{
				case Core.Types.User.UserAuthorizationState.Connected:
					OnConnected();
					break;
				case Core.Types.User.UserAuthorizationState.Disconnected:
					OnDisconnected();
					break;
			}
		}

		private static void OnConnected()
		{
			foreach (var service in _clientServices!)
			{
				if (service.ImplementationInstance is ISession sericeSession)
				{
					sericeSession.InitializeSession();
				}
			}
		}

		private static void OnDisconnected()
		{
			foreach (var service in _clientServices!)
			{
				if (service.ImplementationInstance is ISession sericeSession)
				{
					sericeSession.ClearSession();
				}
			}
		}
	}
}
