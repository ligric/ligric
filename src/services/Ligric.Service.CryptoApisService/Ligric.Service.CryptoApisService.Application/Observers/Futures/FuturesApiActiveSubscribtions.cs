using System.Collections.ObjectModel;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures
{
	public class FuturesApiActiveSubscribtions
	{
		private readonly Dictionary<Guid, long> _usersSubscriptions = new Dictionary<Guid, long>();

		/// <remarks>
		/// <c>Guid</c> key - subscription id<br/>
		/// <c>long</c> value - user id
		/// </remarks>
		public ReadOnlyDictionary<Guid, long> UsersSubscribtions => new ReadOnlyDictionary<Guid, long>(_usersSubscriptions);

		public void AttachSubscription(long userId, out Guid subscriptionId)
		{
			subscriptionId = Guid.NewGuid();
			_usersSubscriptions.TryAdd(subscriptionId, userId);
		}

		public bool DetachSubscribtion(Guid userSubscribtion)
		{
			return _usersSubscriptions.Remove(userSubscribtion);
		}
	}
}
