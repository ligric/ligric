using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IOrdersService : IDisposable 
	{
		IReadOnlyDictionary<long, ExchangedEntity<FuturesOrderDto>> OpenOrders { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesOrderDto>>> OpenOrdersChanged;

		Task AttachStreamAsync(long userApiId);

		void DetachStream(long userApiId);
	}
}
