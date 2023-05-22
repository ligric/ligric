using Utils;

namespace Ligric.Business.Futures
{
	public interface IFuturesTradesService : IDisposable
	{
		IReadOnlyDictionary<string, decimal> Trades { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>> TradesChanged;
	}
}
