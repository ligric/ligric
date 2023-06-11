using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces
{
	public interface ILeverageSubscriptions
	{
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)> GetLeveragesAsObservable();

		void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId, out Guid chainSessionId);

		void UnsubscribeStream(Guid subscribedStreamId);
	}
}
