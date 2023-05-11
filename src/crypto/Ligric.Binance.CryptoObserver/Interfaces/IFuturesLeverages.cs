using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesLeverages : IFuturesLeveragesUpdatedFromPositions
	{
		ReadOnlyDictionary<string, byte> Leverages { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, byte>>? LeveragesChanged;
	}

	public interface IFuturesLeveragesUpdatedFromPositions
	{
		internal Task UpdateLeveragesFromAddedPosition(FuturesPositionDto addedPosition);
	}
}
