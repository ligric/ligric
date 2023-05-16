using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IPositionsService : IDisposable 
	{
		IReadOnlyDictionary<long, ExchangedEntity<FuturesPositionDto>> Positions { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesPositionDto>>> PositionsChanged;

		Task AttachStreamAsync(long userApiId);

		void DetachStream(long userApiId);
	}
}
