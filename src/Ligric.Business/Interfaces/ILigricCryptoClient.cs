using System;
using Ligric.Business.Futures;

namespace Ligric.Business.Interfaces
{
	public interface ILigricCryptoClient : IDisposable
	{
		IOrdersService Orders { get; }

		IValuesService Values { get; }
	}
}
