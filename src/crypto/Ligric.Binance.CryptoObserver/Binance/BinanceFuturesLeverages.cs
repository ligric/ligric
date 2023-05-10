using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesLeverages : IFuturesLeverages
	{
#pragma warning disable CS0414 // The field 'BinanceFuturesLeverages.eventSync' is assigned but its value is never used
		private int eventSync = 0;
#pragma warning restore CS0414 // The field 'BinanceFuturesLeverages.eventSync' is assigned but its value is never used

		private readonly BinanceClient _client;
		private readonly IFuturesPositions _futuresPositions;

		private Dictionary<long, byte> _leverages = new Dictionary<long, byte>();

		internal BinanceFuturesLeverages(BinanceClient client, IFuturesPositions futuresPositions)
		{
			_client = client;
			_futuresPositions = futuresPositions;
		}

		public ReadOnlyDictionary<long, byte> Leverages => new ReadOnlyDictionary<long, byte>(_leverages);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, byte>>? LeveragesChanged;

		internal async Task SetupPrimaryLeveragesAsync(CancellationToken token)
		{
			//var ordersResponse = await _client.UsdFuturesApi.Account.GetPositionInformationAsync(ct: token);
			//var openPositions = ordersResponse.Data
			//	.Where(x => x.EntryPrice > 0)
			//	.Select(binancePosition =>
			//	{
			//		OrderSide side = binancePosition.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
			//		return binancePosition.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side);
			//	})
			//	.ToList();

			//lock (((ICollection)_positions).SyncRoot)
			//{
			//	foreach (var position in openPositions)
			//	{
			//		_positions.AddAndRiseEvent(this, PositionsChanged, position.Id, position, ref eventSync);
			//	}
			//}
		}

		internal void OnLeveragesUpdated(DataEvent<BinanceFuturesStreamConfigUpdate> obj)
		{

		}
	}
}
