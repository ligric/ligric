using Ligric.Core.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IFuturesLeveragesService : IDisposable
	{
		IReadOnlyDictionary<string, LeverageDto> Leverages { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, LeverageDto>> LeveragesChanged;
	}
}
