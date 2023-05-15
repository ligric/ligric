using System.Collections.Specialized;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;

namespace Ligric.Business.Futures
{
	public interface ILeveragesService : IDisposable
	{
		IReadOnlyCollection<ExchangedEntity<LeverageDto>> Leverages { get; }

		event NotifyCollectionChangedEventHandler? LeveragesChanged;
		Task AttachStreamAsync(long userApiId);
		void DetachStream();
	}
}
