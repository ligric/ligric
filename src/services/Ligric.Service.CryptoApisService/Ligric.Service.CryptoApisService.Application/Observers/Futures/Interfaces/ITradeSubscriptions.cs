using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces
{
	public interface ITradeSubscriptions
	{
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetTradesAsObservable();

		void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId, out Guid chainSessionId);

		void UnsubscribeStream(Guid subscribedStreamId);
	}
}
