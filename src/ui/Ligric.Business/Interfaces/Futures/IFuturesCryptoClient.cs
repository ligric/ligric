using Ligric.Business.Futures;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Interfaces
{
	public interface IFuturesCryptoClient : IDisposable
	{
		Guid ClientId { get; }

		IFuturesOrdersService Orders { get; }

		IFuturesTradesService Trades { get; }

		IFuturesPositionsService Positions { get; }

		IFuturesLeveragesService Leverages { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesOrderDto>>>? ClientOrdersChanged;

		event EventHandler<NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesPositionDto>>>? ClientPositionsChanged;

		event EventHandler<NotifyDictionaryChangedEventArgs<string, IdentityEntity<LeverageDto>>>? ClientLeveragesChanged;

		void AttachStream();

		void DetachStream();
	}
}
