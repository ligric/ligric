using System.Collections;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Ligric.CryptoObserver.Extensions;
using Ligric.CryptoObserver.Interfaces;
using Utils;
using Utils.CollectionExtensions;
using Utils.Extensions;

namespace Ligric.CryptoObserver.Binance
{
	public class BinanceFuturesPositions : IFuturesPositions
	{
		private int eventSync = 0;

		private readonly BinanceClient _client;
		private readonly IFuturesOrdersCashed _cashedOrders;
		private readonly IFuturesLeverages _leverages;

		private readonly Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();

		internal BinanceFuturesPositions(
			BinanceClient client,
			IFuturesOrdersCashed cashedOrders,
			IFuturesLeverages leverages)
		{
			_client = client;
			_cashedOrders = cashedOrders;
			_leverages = leverages;
		}

		public ReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		internal async Task SetupPrimaryPositionsAsync(CancellationToken token)
		{
			var positionssResponse = await _client.UsdFuturesApi.Account.GetPositionInformationAsync(ct: token);

			var openPositions = positionssResponse.Data
				.Where(x => x.EntryPrice > 0);

			var positionDto = openPositions.Select(binancePosition =>
			{
				 Side side = binancePosition.Quantity > 0 ? Side.Buy : Side.Sell;
				 byte? leverage = _leverages.Leverages.TryGetValue(binancePosition.Symbol, out byte leverageOut) ? leverageOut : null;

				 return binancePosition.ToFuturesPositionDto(
					 (long)RandomHelper.GetRandomUlong(),
					 side,
					 leverage,
					 binancePosition.Quantity);

			});

			lock (((ICollection)_positions).SyncRoot)
			{
				positionDto.ForEach(position =>
				{
					 _positions.AddAndRiseEvent(this, PositionsChanged, position.Id, position, ref eventSync);
					 _leverages.UpdateLeveragesFromAddedPosition(position);
				});
			}
		}

		internal void OnAccountUpdated(DataEvent<BinanceFuturesStreamAccountUpdate> account)
		{
			var positions = account.Data.UpdateData.Positions;
			foreach (var position in positions)
			{
				if (position.Quantity == 0)
				{
					RemovePosition(position.Symbol);
					continue;
				}
				AddOrSetPosition(position);
			}
		}

		private void RemovePosition(string symbol)
		{
			FuturesPositionDto? remodingItem = _positions.Values.FirstOrDefault(x => x.Symbol == symbol);

			if (remodingItem != null && _positions.TryGetValue(remodingItem.Id, out var removingPosition))
			{
				_positions.RemoveAndRiseEvent(this, PositionsChanged, removingPosition.Id, removingPosition, ref eventSync);
				System.Diagnostics.Debug.WriteLine($"Removed {removingPosition.Symbol}");
			}
		}

		private void AddOrSetPosition(BinanceFuturesStreamPosition position)
		{
			Side side = position.PositionSide.ToSideDto(position.Quantity);

			byte? leverage = _leverages.Leverages.TryGetValue(position.Symbol, out byte leverageOut) ? leverageOut : null;
			FuturesPositionDto? existingItem = _positions.Values.FirstOrDefault(x => x.Symbol == position.Symbol && x.Side == side);

			if (existingItem == null)
			{
				FuturesPositionDto positionDto = position.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side, leverage);
				_positions.AddAndRiseEvent(this, PositionsChanged, positionDto.Id, positionDto, ref eventSync);
				_leverages.UpdateLeveragesFromAddedPosition(positionDto);
			}
			else
			{
				FuturesPositionDto positionDto = position.ToFuturesPositionDto(existingItem.Id, side, leverage);
				_positions.SetAndRiseEvent(this, PositionsChanged, positionDto.Id, positionDto, ref eventSync);
			}
		}
	}
}
