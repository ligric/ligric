using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.Business.Interfaces
{
	public interface ILigricCryptoClient : IDisposable
	{
		IApiesService Apis { get; }

		IFuturesOrdersService Orders { get; }

		IValuesService Values { get; }

		IPositionsService Positions { get; }
	}
}
