using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface ILeveragesService : IDisposable
	{
		IReadOnlyDictionary<Guid, LeverageDto> Leverages { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<Guid, LeverageDto>> LeveragesChanged;

		Task AttachStreamAsync(long userApiId);
	}
}
