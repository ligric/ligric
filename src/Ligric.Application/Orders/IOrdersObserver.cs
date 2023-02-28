using Ligric.Domain.Types.Api;
using Ligric.Domain.Types.Future;
using System;
using Utils;

namespace Ligric.Application.Orders
{
	public interface IOrdersObserverManager
	{
		IObservable<(EventAction Action, long UserId, FuturesOrderDto order)> GetOrdersAsObservable(long userId, long userApiId);
	}
}
