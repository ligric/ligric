using System.Collections.ObjectModel;
using Ligric.CryptoObserver.Binance.Types;

namespace Ligric.CryptoObserver.Interfaces
{
	public interface IFuturesOrdersCashed
	{
		ReadOnlyCollection<BinanceFuturesFilledOrder> LastFilledOrders { get; }
	}
}
