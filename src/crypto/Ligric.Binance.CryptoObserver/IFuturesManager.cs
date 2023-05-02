using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.CryptoObserver
{
	public interface IFuturesManager
	{
		event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;
		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OrdersChanged;
		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		ReadOnlyDictionary<string, decimal> Values { get; }
		ReadOnlyDictionary<long, FuturesOrderDto> Orders { get; }
		ReadOnlyDictionary<long, FuturesPositionDto> Positions { get; }

		Task AttachOrdersSubscribtionsAsync();
	}
}
