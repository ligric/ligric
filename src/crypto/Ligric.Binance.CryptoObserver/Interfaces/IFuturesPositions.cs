using Ligric.Core.Types.Future;
using System.Collections.ObjectModel;
using Utils;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesPositions
	{
		public ReadOnlyDictionary<long, FuturesPositionDto> Positions { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;
	}
}
