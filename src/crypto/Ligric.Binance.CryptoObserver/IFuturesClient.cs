using Ligric.CryptoObserver.Binance;
using Ligric.CryptoObserver.Interfaces;

namespace Ligric.CryptoObserver
{
	public interface IFuturesClient : IDisposable
	{
		IFuturesOrders Orders { get; }

		IFuturesPositions Positions { get; }

		IFuturesTrades Trades { get; }

		IFuturesLeverages Leverages { get; }

		Task StartStreamAsync();

		void StopStream();
	}
}
