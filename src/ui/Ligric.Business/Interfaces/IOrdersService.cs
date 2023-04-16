using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IOrdersService : IDisposable 
	{
		IReadOnlyDictionary<long, FuturesOrderDto> OpenOrders { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>> OpenOrdersChanged;

		Task AttachStreamAsync(long userApiId);

		void DetachStream();
	}
}
