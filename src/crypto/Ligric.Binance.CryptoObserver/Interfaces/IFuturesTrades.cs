using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesTrades
	{
		ReadOnlyDictionary<string, TradeDto[]> Trades { get; }

		event EventHandler<TradeDto>? TradeItemAdded;
	}
}
