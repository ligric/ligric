using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures
{
	public interface IFuturesApiSubscribtionsObserverManager
	{
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId);
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetValuesAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId);
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId);
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)> GetLeveragesAsObservable(long userId, long userApiId, out Guid exchangedId, out Guid subscribtionId);
		void UnsubscribeIdAndTryToRemoveApiSubscribtionObject(Guid subscribtionId);
	}
}
