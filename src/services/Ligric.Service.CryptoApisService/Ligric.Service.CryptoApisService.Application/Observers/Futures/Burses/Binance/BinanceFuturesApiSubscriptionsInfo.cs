using Binance.Net.Objects;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinanceFuturesApiSubscriptionsInfo
	{
		public BinanceFuturesApiSubscriptionsInfo(ApiDto api, BinanceApiCredentials credentials, bool isTest = true)
		{
			FuturesBurseClient = new BinanceFuturesApiSubscriptionsBurseSessionWrapper(api, credentials, isTest);
			ActiveSubscribtions = new FuturesApiActiveSubscribtions();
		}

		public IFuturesApiSubscriptionsBurseSessionWrapper FuturesBurseClient { get; }
		 
		public FuturesApiActiveSubscribtions ActiveSubscribtions { get; }
	}
}
