using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IFuturesPositionsService : IDisposable 
	{
		IReadOnlyDictionary<long, FuturesPositionDto> Positions { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>> PositionsChanged;
	}
}
