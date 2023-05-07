using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.CryptoObserver.Interfaces
{
	internal interface IFuturesOrders
	{
		ReadOnlyDictionary<long, FuturesOrderDto> Orders { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OrdersChanged;
	}
}
