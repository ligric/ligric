using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Service.CryptoApisService.Application
{
	public interface ITemporaryUserFuturesObserver
	{
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId, out Guid subscribtionId);
		IObservable<NotifyDictionaryChangedEventArgs<string, decimal>> GetValuesAsObservable(long userId, long userApiId, out Guid subscribtionId);
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId, out Guid subscribtionId);
		IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)> GetLeveragesAsObservable(long userId, long userApiId, out Guid subscribtionId);
		void UnsubscribeUser(Guid subscribtionId);
	}
}
