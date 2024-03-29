﻿using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Application.Orders
{
	public interface ITemporaryUserFuturesObserver
	{
		IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> EventArgs)> GetOrdersAsObservable(long userId, long userApiId);
		IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<string, decimal> EventArgs)> GetValuesAsObservable(long userId, long userApiId);
		IObservable<(IEnumerable<long> UserIds, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> EventArgs)> GetPositionsAsObservable(long userId, long userApiId);
	}
}
