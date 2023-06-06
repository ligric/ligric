using Binance.Net.Objects;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinanceFuturesApiSubscriptions : IFuturesApiSubscriptions
	{
		/// <summary>
		/// <c>long</c>> value -- ApiId
		/// </summary>
		private static readonly Dictionary<long, BinanceFuturesApiSubscriptionsInfo> subscribedApis = new Dictionary<long, BinanceFuturesApiSubscriptionsInfo>();

		public void AttachSubscriptionIdToApi(ApiDto api, long userId, out Guid subscriptionId, out IFuturesApiSubscriptionsBurseSessionWrapper bursSession)
		{
			BinanceFuturesApiSubscriptionsInfo subscribtionsInfo;
			if (subscribedApis.TryGetValue(api.Id, out subscribtionsInfo))
			{
				subscribtionsInfo.ActiveSubscribtions.AttachSubscription(userId, out subscriptionId);
			}
			else
			{
				subscribtionsInfo = new BinanceFuturesApiSubscriptionsInfo(api, new BinanceApiCredentials(api.PublicKey, api.PrivateKey));
				subscribtionsInfo.ActiveSubscribtions.AttachSubscription(userId, out subscriptionId);
				subscribedApis.Add(api.Id, subscribtionsInfo);
			}
			bursSession = subscribtionsInfo.FuturesBurseClient;
		}

		/// <summary>
		/// Will unsubscribe subscribtion from his Id.
		/// Will remove the <see cref="BinanceFuturesApiSubscriptionsBurseSessionWrapper"/> if noone subscribed to this API/>
		/// </summary>
		public void DetachSubscriptionAndTryToRemoveApiSubscriptionObject(Guid subscribtionId)
		{
			BinanceFuturesApiSubscriptionsInfo? removedApi = null;

			foreach (var subscribedApi in subscribedApis.Values)
			{
				var activeSubscribtions = subscribedApi.ActiveSubscribtions;
				if (activeSubscribtions.UsersSubscribtions.ContainsKey(subscribtionId))
				{
					//lock (((ICollection)activeSubscribtions.UsersSubscribtions).SyncRoot)
					{
						subscribedApi.ActiveSubscribtions.DetachSubscribtion(subscribtionId);
						if (subscribedApi.ActiveSubscribtions.UsersSubscribtions.Count == 0)
						{
							removedApi = subscribedApi;
						}
						break;
					}
				}

				if (removedApi?.FuturesBurseClient.Api != null)
				{
					subscribedApis.Remove((long)removedApi?.FuturesBurseClient.Api.Id!);
					try
					{
						removedApi.FuturesBurseClient!.Dispose();
					}
					catch (ObjectDisposedException ex)
					{
						var aaaa = ex;
						System.Diagnostics.Debug.WriteLine("Subscribtion already disposed.");
					}
					catch
					{
						throw;
					}
					System.Diagnostics.Debug.WriteLine($"Api was fully removed.");
				}
			}
		}
	}
}
