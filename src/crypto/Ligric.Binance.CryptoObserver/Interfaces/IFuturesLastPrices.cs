using System.Collections.ObjectModel;
using Utils;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesLastPrices
	{
		ReadOnlyDictionary<string, decimal> LastValues { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? LastValuesChanged;
	}
}
