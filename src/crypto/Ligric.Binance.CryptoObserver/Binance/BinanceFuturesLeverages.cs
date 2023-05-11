using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Interfaces;
using Utils;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesLeverages : IFuturesLeverages
	{
		private int eventSync = 0;

		private readonly BinanceClient _client;

		private readonly Dictionary<string, byte> _leverages = new Dictionary<string, byte>();

		internal BinanceFuturesLeverages(BinanceClient client)
		{
			_client = client;
		}

		public ReadOnlyDictionary<string, byte> Leverages => new ReadOnlyDictionary<string, byte>(_leverages);

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, byte>>? LeveragesChanged;

		private async Task SetupPrimaryLeveragesAsync(string symbol)
		{
			if (_leverages.ContainsKey(symbol)) return;

			var positionsResponse = await _client.UsdFuturesApi.Account.GetPositionInformationAsync(symbol);
			var position = positionsResponse.Data.First();
			lock (((ICollection)_leverages).SyncRoot)
			{
				_leverages.EqualBeforeAddOrSetAndRiseEvent(this, LeveragesChanged, symbol, (byte)position.Leverage, ref eventSync);
			}
		}

		internal void OnLeveragesUpdated(DataEvent<BinanceFuturesStreamConfigUpdate> obj)
		{
			BinanceFuturesStreamLeverageUpdateData leverage = obj.Data.LeverageUpdateData;
			lock (((ICollection)_leverages).SyncRoot)
			{
				_leverages.EqualBeforeAddOrSetAndRiseEvent(this, LeveragesChanged, leverage.Symbol!, (byte)leverage.Leverage, ref eventSync);
			}
		}

		async Task IFuturesLeveragesUpdatedFromPositions.UpdateLeveragesFromAddedPosition(FuturesPositionDto addedPosition)
		{
			await SetupPrimaryLeveragesAsync(addedPosition.Symbol);
		}
	}
}
