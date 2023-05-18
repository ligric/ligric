using System.Collections.Specialized;
using Ligric.Core.Types.Future;

namespace Ligric.Business.Futures
{
	public interface IFuturesLeveragesService : IDisposable
	{
		IReadOnlyCollection<LeverageDto> Leverages { get; }

		event NotifyCollectionChangedEventHandler? LeveragesChanged;
	}
}
