using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Service.CryptoApisService.Application
{
	public interface ITemporaryUserFuturesObserver
	{
		IObservable<(IEnumerable<long> UserIds, Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId);
		IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetValuesAsObservable(long userId, long userApiId);
		IObservable<(IEnumerable<long> UserIds, Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId);
		IObservable<(IEnumerable<long> UserIds, Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, byte> EventArgs)> GetLeveragesAsObservable(long userId, long userApiId);
	}
}
