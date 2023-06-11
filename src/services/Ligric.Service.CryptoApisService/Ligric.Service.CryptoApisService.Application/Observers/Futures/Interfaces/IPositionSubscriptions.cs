using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces
{
	public interface IPositionSubscriptions
	{
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable();

		void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId, out Guid chainSessionId);

		void UnsubscribeStream(Guid subscribedStreamId);
	}
}
