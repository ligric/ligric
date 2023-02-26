using Ligric.Business.Apies;
using Ligric.Business.Authorization;

namespace Ligric.Business.Subscriptions
{
	public class SubscribeWebSockets : ISubscribeWebSockets
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly IApiesService _userApiService;

		public SubscribeWebSockets(
			IAuthorizationService authorizationService,
			IApiesService userApiService)
		{
			_authorizationService = authorizationService;
			_userApiService = userApiService;
		}

		public void AttachAll()
		{
			_userApiService.ApiPiplineSubscribeAsync();
		}

		public void DetachAll()
		{
			_userApiService.ApiPiplineUnsubscribe();
		}
	}
}
