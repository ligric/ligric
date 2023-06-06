using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces
{
	public interface IFuturesApiSubscriptionsBurseSessionWrapper : IDisposable
	{
		Guid BurseSessionId { get; }

		ApiDto Api { get; }

		IFuturesClient FuturesClient { get; }

		event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> OrderEventArgs)>? OrdersChanged;

		event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, decimal>)>? TradesChanged;

		event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> valueEventArgs)>? PositionsChanged;

		event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> leverageEventArgs)>? LeveragesChanged;
	}
}
