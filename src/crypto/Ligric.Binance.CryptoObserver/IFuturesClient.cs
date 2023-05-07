﻿using Ligric.CryptoObserver.Binance;
using Ligric.CryptoObserver.Interfaces;

namespace Ligric.CryptoObserver
{
	public interface IFuturesClient
	{
		IFuturesOrders Orders { get; }

		IFuturesPositions Positions { get; }

		IFuturesValues Values { get; }

		Task StartStreamAsync();

		void StopStream();
	}
}