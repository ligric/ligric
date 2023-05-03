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

		var startStreamResponse = await _client.UsdFuturesApi.Account.StartUserStreamAsync(token);
		var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

		var updateSubscription = await _socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
			listenKey, null, null,
			OnAccountUpdated, OnOrdersUpdated, OnListenKeyExpired,
			null, null, token);



		//var ordersResponse = await _client.UsdFuturesApi.Trading.GetOpenOrdersAsync(ct: token);
		//_orders = ordersResponse.Data.Select(binanceOrder => binanceOrder.ToFuturesOrderDto()).ToList();

		//foreach (var order in _orders)
		//{

		//}
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
				FuturesPositionDto positionDto = position.ToFuturesPositionDto((long)RandomHelper.GetRandomUlong());
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
