using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Extensions;
using Ligric.CryptoObserver.Interfaces;
using Utils;
using Utils.Extensions;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesPositions : IFuturesPositions
	{
		private int eventSync = 0;

		private readonly BinanceClient _client;

		private Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();

		internal BinanceFuturesPositions(BinanceClient client)
		{
			_client = client;
		}

		public ReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		internal async Task SetupPrimaryPositionsAsync(CancellationToken token)
		{
			var ordersResponse = await _client.UsdFuturesApi.Account.GetPositionInformationAsync(ct: token);
			var openPositions = ordersResponse.Data
				.Where(x => x.EntryPrice > 0)
				.Select(binancePosition =>
				{
					OrderSide side = binancePosition.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
					return binancePosition.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side);
				})
				.ToList();

			lock (((ICollection)_positions).SyncRoot)
			{
				foreach (var position in openPositions)
				{
					_positions.AddAndRiseEvent(this, PositionsChanged, position.Id, position, ref eventSync);
				}
			}
		}

		internal void OnAccountUpdated(DataEvent<BinanceFuturesStreamAccountUpdate> account)
		{
			var positions = account.Data.UpdateData.Positions;
			foreach (var position in positions)
			{
				FuturesPositionDto? existingItem = _positions.Values.FirstOrDefault(x => x.Symbol == position.Symbol);

				if (position.Quantity == 0)
				{
					if (existingItem != null)
					{
						_positions.RemoveAndRiseEvent(this, PositionsChanged, existingItem.Id, ref eventSync);
					}
					continue;
				}

				if (existingItem == null)
				{
					OrderSide side = position.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
					FuturesPositionDto positionDto = position.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side);
					_positions.AddAndRiseEvent(this, PositionsChanged, positionDto.Id, positionDto, ref eventSync);
				}
			}
		}
	}
}
