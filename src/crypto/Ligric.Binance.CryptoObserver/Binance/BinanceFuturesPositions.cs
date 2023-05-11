﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.CommonObjects;
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
		private readonly IFuturesLeverages _leverages;

		private readonly Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();

		internal BinanceFuturesPositions(
			BinanceClient client,
			IFuturesLeverages leverages)
		{
			_client = client;
			_leverages = leverages;
		}

		public ReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

		internal async Task SetupPrimaryPositionsAsync(CancellationToken token)
		{
			var positionssResponse = await _client.UsdFuturesApi.Account.GetPositionInformationAsync(ct: token);
			var openPositions = positionssResponse.Data
				.Where(x => x.EntryPrice > 0)
				.Select(binancePosition =>
				{
					OrderSide side = binancePosition.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
					byte? leverage = _leverages.Leverages.TryGetValue(binancePosition.Symbol, out byte leverageOut) ? leverageOut : null;
					return binancePosition.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side, leverage);
				})
				.ToList();

			lock (((ICollection)_positions).SyncRoot)
			{
				foreach (var position in openPositions)
				{
					_positions.AddAndRiseEvent(this, PositionsChanged, position.Id, position, ref eventSync);
					_leverages.UpdateLeveragesFromAddedPosition(position);
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
					if (existingItem != null && _positions.TryGetValue(existingItem.Id, out var removingPosition))
					{
						_positions.RemoveAndRiseEvent(this, PositionsChanged, removingPosition.Id, removingPosition, ref eventSync);
					}
					continue;
				}

				if (existingItem == null)
				{
					OrderSide side = position.Quantity > 0 ? OrderSide.Buy : OrderSide.Sell;
					byte? leverage = _leverages.Leverages.TryGetValue(position.Symbol, out byte leverageOut) ? leverageOut : null;
					FuturesPositionDto positionDto = position.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong(), side, leverage);
					_positions.AddAndRiseEvent(this, PositionsChanged, positionDto.Id, positionDto, ref eventSync);
					_leverages.UpdateLeveragesFromAddedPosition(positionDto);
				}
			}
		}
	}
}
