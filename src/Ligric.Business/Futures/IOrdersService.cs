using Ligric.Domain.Types.Future;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IOrdersService : IDisposable 
	{
		IReadOnlyDictionary<long, FuturesOrderDto> OpenOrders { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>> OpenOrdersChanged;

		Task SubscribeOpenOrdersFromUserIdAsync(long userApiId);

		Task UnsubscribeOpenOrdersFromUserIdAsync(long userApiId);

		Task AttachStreamAsync();

		void DetachStream();
	}
}
