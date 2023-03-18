using System;
using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.Business.Interfaces
{
	public interface ILigricCryptoClient : IDisposable
	{
		IApiesService Apis { get; }

		IOrdersService Orders { get; }

		IValuesService Values { get; }

		IPositionsService Positions { get; }
	}
}
