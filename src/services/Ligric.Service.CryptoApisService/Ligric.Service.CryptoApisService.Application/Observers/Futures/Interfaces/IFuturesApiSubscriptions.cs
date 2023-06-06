using Ligric.Core.Ligric.Core.Types.Api;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces
{
	public interface IFuturesApiSubscriptions
	{
		void AttachSubscriptionIdToApi(ApiDto api, long userId, out Guid subscriptionId, out IFuturesApiSubscriptionsBurseSessionWrapper bursSession);

		void DetachSubscriptionAndTryToRemoveApiSubscriptionObject(Guid subscribtionId);
	}
}
