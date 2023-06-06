using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces
{
	public interface IOrderSubscriptions
	{
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable();

		void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId);

		void UnsubscribeStream(Guid subscribedStreamId);
	}
}
