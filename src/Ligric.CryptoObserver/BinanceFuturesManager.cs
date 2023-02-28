using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;

namespace Ligric.CryptoObserver;

public class BinanceFuturesManager
{
	private readonly BinanceApiAddresses _address;
	private readonly BinanceApiCredentials _credentials;
	private readonly BinanceClient _client;
	private readonly BinanceSocketClient _socketClient;

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

	public async Task AttachOrdersSubscribtions()
	{
		var startStreamResponse = await _client.UsdFuturesApi.Account.StartUserStreamAsync();
		var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

		await _socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
			listenKey, null, null,
			OnAccountUpdated, OnOrdersUpdated, OnListenKeyExpired,
			null, null);
	}

	private void OnAccountUpdated(DataEvent<BinanceFuturesStreamAccountUpdate> account)
	{
		var data = account.Data.UpdateData;
		//Console.WriteLine($"\n\n\tAccountUpdate\n" +
		//	$"Reason: {data.Reason};\n" +
		//	$"Balances: {string.Join(",", data.Balances.Select(x => $"{{Asset:{x.Asset}, WalletBalance:{x.WalletBalance}, CrossWalletBalance:{x.CrossWalletBalance}, BalanceChange:{x.BalanceChange}}}"))};\n" +
		//	$"Positions: {string.Join(",", data.Positions.Select(x => $"{{Symbol:{x.Symbol}, Quantity:{x.Quantity}, UnrealizedPnl:{x.UnrealizedPnl}, RealizedPnl:{x.RealizedPnl}, EntryPrice:{x.EntryPrice}, PositionSide:{x.PositionSide}, MarginType:{x.MarginType}, IsolatedMargin:{x.IsolatedMargin}}}"))};");
	}

	private void OnOrdersUpdated(DataEvent<BinanceFuturesStreamOrderUpdate> order)
	{
		{
			var data = order.Data.UpdateData;
			//Console.WriteLine($"\n\n\tOrderUpdate\n" +
			//		$"OrderId: {data.OrderId};\n" +
			//		$"Symbol: {data.Symbol};\n" +
			//		$"Side: {data.Side};\n" +
			//		$"Type: {data.Type};\n" +
			//		$"TimeInForce: {data.TimeInForce}\n" +
			//		$"Quantity: {data.Quantity}\n" +
			//		$"Price: {data.Price}\n" +
			//		$"AveragePrice: {data.AveragePrice}\n" +
			//		$"StopPrice: {data.StopPrice}\n" +
			//		$"ExecutionType: {data.ExecutionType}\n" +
			//		$"Status: {data.Status}\n" +
			//		$"QuantityOfLastFilledTrade: {data.QuantityOfLastFilledTrade}\n" +
			//		$"AccumulatedQuantityOfFilledTrades: {data.AccumulatedQuantityOfFilledTrades}\n" +
			//		$"PriceLastFilledTrade: {data.PriceLastFilledTrade}\n" +
			//		$"Fee: {data.Fee}\n" +
			//		$"FeeAsset: {data.FeeAsset}\n" +
			//		$"BidNotional: {data.BidNotional}\n" +
			//		$"AskNotional: {data.AskNotional}\n" +
			//		$"BuyerIsMaker: {data.BuyerIsMaker}\n" +
			//		$"IsReduce: {data.IsReduce}\n" +
			//		$"StopPriceWorking: {data.StopPriceWorking}\n" +
			//		$"OriginalType: {data.OriginalType}\n" +
			//		$"PositionSide: {data.PositionSide}\n" +
			//		$"PushedConditionalOrder: {data.PushedConditionalOrder}\n" +
			//		$"ActivationPrice: {data.ActivationPrice}\n" +
			//		$"CallbackRate: {data.CallbackRate}\n" +
			//		$"RealizedProfit: {data.RealizedProfit}");
		}
	}

	private void OnListenKeyExpired(DataEvent<BinanceStreamEvent> obj)
	{
		System.Diagnostics.Debug.WriteLine("Listen key expired");
	}
}
