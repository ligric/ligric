using System.Collections.ObjectModel;
using Utils;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesValues
	{
		ReadOnlyDictionary<string, decimal> Values { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;
	}
}
