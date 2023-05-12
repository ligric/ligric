using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Objects.Models;
using CryptoExchange.Net.Sockets;
using Ligric.CryptoObserver.Binance;
using Ligric.CryptoObserver.Interfaces;

namespace Ligric.CryptoObserver;

public class BinanceFuturesClient : IFuturesClient
{
	private readonly BinanceClient _client;
	private readonly BinanceSocketClient _socketClient;

	private readonly BinanceFuturesOrders _orders;
	private readonly BinanceFuturesPositions _positions;
	private readonly BinanceFuturesTrades _trades;
	private readonly BinanceFuturesLeverages _leverages;

	private CancellationTokenSource? _streamCancellationToken;

	public BinanceFuturesClient(BinanceApiCredentials credentials, bool isTest = true)
	{
		var address = isTest ? BinanceApiAddresses.TestNet : BinanceApiAddresses.Default;

		_client = new BinanceClient(new BinanceClientOptions()
		{
			ApiCredentials = credentials,
			UsdFuturesApiOptions = new BinanceApiClientOptions()
			{
				BaseAddress = address.UsdFuturesRestClientAddress!,
				AutoTimestamp = true
			}
		});
		_socketClient = new BinanceSocketClient(new BinanceSocketClientOptions()
		{
			UsdFuturesStreamsOptions = new BinanceSocketApiClientOptions(address.UsdFuturesSocketClientAddress!),
			BlvtStreamAddress = address.BlvtSocketClientAddress,
			ApiCredentials = credentials
		});

		_orders = new BinanceFuturesOrders(_client);
		_leverages = new BinanceFuturesLeverages(_client);
		_positions = new BinanceFuturesPositions(_client, _leverages);
		_trades = new BinanceFuturesTrades(_socketClient, _orders, _positions);
	}

	public IFuturesOrders Orders => _orders;

	public IFuturesPositions Positions => _positions;

	public IFuturesTrades Trades => _trades;

	public IFuturesLeverages Leverages => _leverages;

	public async Task StartStreamAsync()
	{
		if (_streamCancellationToken != null
			&& !_streamCancellationToken.IsCancellationRequested)
		{
			return;
		}

		_streamCancellationToken = new CancellationTokenSource();
		var token = _streamCancellationToken.Token;

#pragma warning disable CS4014 // Should be async because it is a request race.
							   // All data is sync later.
		StartFuturesStreamAsync(token);

		_orders.SetupPrimaryOrdersAsync(token);

		_positions.SetupPrimaryPositionsAsync(token);
#pragma warning restore CS4014 // Should be async because it is a request race
	}

	public void StopStream()
	{
		_streamCancellationToken?.Cancel();
		_streamCancellationToken?.Dispose();
	}

	private async Task StartFuturesStreamAsync(CancellationToken token)
	{
		var startStreamResponse = await _client.UsdFuturesApi.Account.StartUserStreamAsync(token);
		var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

		var updateSubscription = await _socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
			listenKey, _leverages.OnLeveragesUpdated, null,
			_positions.OnAccountUpdated, _orders.OnOrdersUpdated, OnListenKeyExpired,
			null, null, token);
	}

	private void OnListenKeyExpired(DataEvent<BinanceStreamEvent> obj)
	{
		System.Diagnostics.Debug.WriteLine("Listen key expired");
	}

	public void Dispose()
	{
		StopStream();
		_trades.Dispose();
		_client.Dispose();
		_socketClient.Dispose();
	}
}
