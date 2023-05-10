using System.Collections.ObjectModel;
using Utils;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesLeverages
	{
		ReadOnlyDictionary<long, byte> Leverages { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, byte>>? LeveragesChanged;
	}
}
