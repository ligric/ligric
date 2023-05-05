using System.Collections.ObjectModel;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Futures.Socket;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.CryptoObserver.Extensions;
using Ligric.Core.Types.Future;
using Utils;
using Utils.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;

namespace Ligric.CryptoObserver;

public class BinanceFuturesManager : IFuturesManager
{
	private int eventSync = 0;

	private readonly BinanceApiAddresses _address;
	private readonly BinanceApiCredentials _credentials;
	private readonly BinanceClient _client;
	private readonly BinanceSocketClient _socketClient;

	private CancellationTokenSource? _ordersSubscribeCancellationToken;

	private Dictionary<string, decimal> _values = new Dictionary<string, decimal>();
	private Dictionary<long, FuturesOrderDto> _orders = new Dictionary<long, FuturesOrderDto>();
	private Dictionary<long, FuturesPositionDto> _positions = new Dictionary<long, FuturesPositionDto>();

	public event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>? ValuesChanged;
	public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OrdersChanged;
	public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>>? PositionsChanged;

	public BinanceFuturesManager(BinanceApiCredentials credentials, bool isTest = true)
	{
		_credentials = credentials;

		_address = isTest ? BinanceApiAddresses.TestNet : BinanceApiAddresses.Default;

		_client = new BinanceClient(new BinanceClientOptions()
		{
			ApiCredentials = _credentials,
			UsdFuturesApiOptions = new BinanceApiClientOptions()
			{
				BaseAddress = _address.UsdFuturesRestClientAddress!,
				AutoTimestamp = true
			}
		});

		_socketClient = new BinanceSocketClient(new BinanceSocketClientOptions()
		{
			UsdFuturesStreamsOptions = new BinanceSocketApiClientOptions(_address.UsdFuturesSocketClientAddress!),
			BlvtStreamAddress = _address.BlvtSocketClientAddress,
			ApiCredentials = _credentials
		});
	}

	public ReadOnlyDictionary<string, decimal> Values => new ReadOnlyDictionary<string, decimal>(_values);

	public ReadOnlyDictionary<long, FuturesOrderDto> Orders => new ReadOnlyDictionary<long, FuturesOrderDto>(_orders);

	public ReadOnlyDictionary<long, FuturesPositionDto> Positions => new ReadOnlyDictionary<long, FuturesPositionDto>(_positions);

	public async Task AttachOrdersSubscribtionsAsync()
	{
		if (_ordersSubscribeCancellationToken != null
				&& !_ordersSubscribeCancellationToken.IsCancellationRequested)
		{
			return;
		}
		_ordersSubscribeCancellationToken = new CancellationTokenSource();
		var token = _ordersSubscribeCancellationToken.Token;

		await StartStream(token);

		await SetupPrimaryOrders(token);

		await SetupPrimaryPositions(token);
	}

	private async Task StartStream(CancellationToken token)
	{
		var startStreamResponse = await _client.UsdFuturesApi.Account.StartUserStreamAsync(token);
		var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

		var updateSubscription = await _socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
			listenKey, null, null,
			OnAccountUpdated, OnOrdersUpdated, OnListenKeyExpired,
			null, null, token);
	}

	private async Task SetupPrimaryOrders(CancellationToken token)
	{
		var ordersResponse = await _client.UsdFuturesApi.Trading.GetOpenOrdersAsync(ct: token);
		var orders = ordersResponse.Data
			.Select(binanceOrder => binanceOrder.ToFuturesOrderDto())
			.ToList();

		lock(((ICollection)_orders).SyncRoot)
		{
			foreach (var order in orders)
			{
				_orders.AddAndRiseEvent(this, OrdersChanged, order.Id, order, ref eventSync);
			}
		}
	}

	private async Task SetupPrimaryPositions(CancellationToken token)
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

	private async Task SubscribeValuesUpdateAsync(string symbol)
	{
		bool isAdded = false;
		lock(((ICollection)_values).SyncRoot)
		{
			if (!_values.ContainsKey(symbol))
			{
				_values.Add(symbol, -1);
				isAdded = true;
			}
		}

		if (isAdded)
		{
			await _socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(symbol, OnAggregatedUpdated);
		}
	}

	private void OnAggregatedUpdated(DataEvent<BinanceStreamAggregatedTrade> obj)
	{
		var data = obj.Data;
		_values.SetAndRiseEvent(this, ValuesChanged, data.Symbol, data.Price, ref eventSync);
	}

	private void OnAccountUpdated(DataEvent<BinanceFuturesStreamAccountUpdate> account)
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

	private void OnOrdersUpdated(DataEvent<BinanceFuturesStreamOrderUpdate> order)
	{
		BinanceFuturesStreamOrderUpdateData streamOrder = order.Data.UpdateData;
		FuturesOrderDto orderDto = streamOrder.ToFuturesOrderDto();

		if (streamOrder.Status is OrderStatus.New)
		{
			_socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(orderDto.Symbol, OnAggregatedUpdated);

			_orders.AddAndRiseEvent(this, OrdersChanged, orderDto.Id, orderDto, ref eventSync);
			return;
		}
		_orders.RemoveAndRiseEvent(this, OrdersChanged, orderDto.Id, ref eventSync);
	}

	private void OnListenKeyExpired(DataEvent<BinanceStreamEvent> obj)
	{
		System.Diagnostics.Debug.WriteLine("Listen key expired");
	}
}
