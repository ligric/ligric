using Ligric.Core.Types.Future;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesLastPrices
	{
		TradeDto[] LastValues { get; }

		event EventHandler<TradeDto>? LastValueItemAdded;
	}
}
