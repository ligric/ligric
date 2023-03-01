using Ligric.Domain.Types.Api;
using Ligric.Domain.Types.Future;
using System;
using Utils;

namespace Ligric.Application.Orders
{
	public interface IFuturesObserver
	{
		IObservable<(EventAction Action, long UserId, FuturesOrderDto Order)> GetOrdersAsObservable(long userId, long userApiId);
	}
}
