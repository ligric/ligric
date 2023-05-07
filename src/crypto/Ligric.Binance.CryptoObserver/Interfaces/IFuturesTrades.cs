using Ligric.Core.Types.Future;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesTrades
	{
		TradeDto[] LastValues { get; }

		event EventHandler<TradeDto>? LastValueItemAdded;
	}
}
