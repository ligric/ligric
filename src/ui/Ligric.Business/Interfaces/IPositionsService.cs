using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IPositionsService : IDisposable 
	{
		IReadOnlyDictionary<long, FuturesPositionDto> Positions { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>> PositionsChanged;

		Task AttachStreamAsync(long userApiId);

		void DetachStream();
	}
}
