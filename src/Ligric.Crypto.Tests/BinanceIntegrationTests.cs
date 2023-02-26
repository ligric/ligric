using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Principal;
using Binance.Net.Clients;
using Binance.Net.Clients.UsdFuturesApi;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Futures.Socket;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ligric.Crypto.Tests
{
	public class BinanceIntegrationTests
	{
		private const string PUBLICK_KEY = "c58577a8b8d83617fb678838fa8e43c83e53384e88fef416c81658e51c6c48f3";
		private const string PRIVATE_KEY = "651096d67c3d1a080daf6d26a37ad545864d312b7a6b24d5f654d4f26a1a7ddc";

		[Fact]
		public async void Binance_Authorization_True()
		{
			//BinanceClient.SetDefaultOptions(new BinanceClientOptions(BinanceApiAddresses.TestNet)
			//{
			//	ApiCredentials = new BinanceApiCredentials(PUBLICK_KEY, PRIVATE_KEY),
			//	LogLevel = LogLevel.Debug,
			//	LogWriters = new List<ILogger>()
			//});



			var binanceClientOptions = new BinanceClientOptions()
			{
				ApiCredentials = new BinanceApiCredentials(PUBLICK_KEY, PRIVATE_KEY),
				UsdFuturesApiOptions = new BinanceApiClientOptions()
				{
					BaseAddress = BinanceApiAddresses.TestNet.UsdFuturesRestClientAddress ?? string.Empty,
					AutoTimestamp = true
				}
			};

			var binanceClient = new BinanceClient(binanceClientOptions);
		    var startStreamResponse = await binanceClient.UsdFuturesApi.Account.StartUserStreamAsync();
			var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

			var testSocket = new BinanceSocketClientOptions()
			{
				BlvtStreamAddress = BinanceApiAddresses.TestNet.BlvtSocketClientAddress,
				ApiCredentials = new BinanceApiCredentials(PUBLICK_KEY, PRIVATE_KEY),
				LogLevel = LogLevel.Debug,
				LogWriters = new List<ILogger>()
			};

			BinanceSocketClient.SetDefaultOptions(testSocket);
			var client = new BinanceSocketClient(testSocket);

			// TODO : A cup of pair changes
			// AggregatedTrade 	 Id: 1608031653, Symbol: BTCUSDT, Price: 24871.60, BuyerIsMaker: False, Quantity: 0.099
			//_ = client.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync("BTCUSDT", x =>
			//{
			//	BinanceStreamAggregatedTrade data = x.Data;
			//	Debug.WriteLine($"AggregatedTrade \t Id: {data.Id}, Symbol: {data.Symbol}, Price: {data.Price}, BuyerIsMaker: {data.BuyerIsMaker}, Quantity: {data.Quantity}");
			//});

			// BookTicker 	 UpdateId: 2530186898250, Symbol: BTCUSDT, BestBidPrice: 24865.40, BestAskPrice: 24865.50, BestBidQuantity: 29.535, BestAskQuantity: 2.718
			//_ = client.UsdFuturesStreams.SubscribeToBookTickerUpdatesAsync("BTCUSDT", x =>
			//{
			//	BinanceFuturesStreamBookPrice data = x.Data;
			//	Debug.WriteLine($"BookTicker \t UpdateId: {data.UpdateId}, Symbol: {data.Symbol}, BestBidPrice: {data.BestBidPrice}, BestAskPrice: {data.BestAskPrice}, BestBidQuantity: {data.BestBidQuantity}, BestAskQuantity: {data.BestAskQuantity}");
			//});

			await client.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
				listenKey,
				leverage =>
				{

				},
				margin =>
				{

				},

				account =>
				{
					var data = account.Data;
					//Debug.WriteLine($"AccountUpdate \t Id: {data.}, Symbol: {data.Symbol}, Price: {data.Price}, BuyerIsMaker: {data.BuyerIsMaker}, Quantity: {data.Quantity}");
				},
				order =>
				{
					var data = order.Data;
					//Debug.WriteLine($"OrderUpdate \t Id: {data.UpdateData}, Symbol: {data.Symbol}, Price: {data.Price}, BuyerIsMaker: {data.BuyerIsMaker}, Quantity: {data.Quantity}");
				},

				listenKeyExpired =>
				{

				},
				strategy =>
				{

				},
				grid =>
				{

				});

			await Task.Delay(500_000);
		}
	}
}
