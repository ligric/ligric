using Ligric.Business.Futures;

namespace Ligric.Business.Interfaces
{
	public interface IFuturesCryptoClient : IDisposable
	{
		IFuturesOrdersService Orders { get; }

		IFuturesTradesService Trades { get; }

		IFuturesPositionsService Positions { get; }

		IFuturesLeveragesService Leverages { get; }

		Task AttachStreamAsync();

		void DetachStream();
	}
}
