using Ligric.Domain.Types.Future;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ligric.CryptoObserver
{
	public interface IFuturesObserber
	{
		public ReadOnlyDictionary<string, decimal> Values { get; }

		public ReadOnlyDictionary<long, FuturesOrderDto> Orders { get; }

		public ReadOnlyDictionary<long, FuturesPositionDto> Positions { get; }

		Task AttachOrdersSubscribtionsAsync();
	}
}
