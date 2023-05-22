using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IFuturesOrdersService : IDisposable 
	{
		IReadOnlyDictionary<long, FuturesOrderDto> Orders { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>> OrdersChanged;
	}
}
