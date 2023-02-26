using System.Diagnostics;
using Binance.Net.Clients;
using Binance.Net.Objects;

Console.Write("PublickKey: ");
var publicKey = Console.ReadLine();
Console.Write("PrivateKey: ");
var privateKey = Console.ReadLine();


string PUBLICK_KEY = publicKey == null || publicKey == "" ? "c58577a8b8d83617fb678838fa8e43c83e53384e88fef416c81658e51c6c48f3" : publicKey;
string PRIVATE_KEY = privateKey == null || privateKey =="" ? "651096d67c3d1a080daf6d26a37ad545864d312b7a6b24d5f654d4f26a1a7ddc" : privateKey;
Console.WriteLine("Loading...");

var credintials = new BinanceApiCredentials(PUBLICK_KEY, PRIVATE_KEY);

BinanceClient.SetDefaultOptions(new BinanceClientOptions()
{
	ApiCredentials = credintials,
	UsdFuturesApiOptions = new BinanceApiClientOptions()
	{
		BaseAddress = BinanceApiAddresses.TestNet.UsdFuturesRestClientAddress!,
		AutoTimestamp = true
	}
});

BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
{
	UsdFuturesStreamsOptions = new BinanceSocketApiClientOptions(BinanceApiAddresses.TestNet.UsdFuturesSocketClientAddress!),
	BlvtStreamAddress = BinanceApiAddresses.TestNet.BlvtSocketClientAddress,
	ApiCredentials = credintials
});

var client = new BinanceClient();
var socketClient = new BinanceSocketClient();

var startStreamResponse = await client.UsdFuturesApi.Account.StartUserStreamAsync();
var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();


await socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
	listenKey,
	leverage =>
	{

	},
	margin =>
	{

	},

	account =>
	{
		var data = account.Data.UpdateData;
		Console.WriteLine($"\n\n\tAccountUpdate\n" +
			$"Reason: {data.Reason};\n" +
			$"Balances: {string.Join(",", data.Balances.Select(x => $"{{Asset:{x.Asset}, WalletBalance:{x.WalletBalance}, CrossWalletBalance:{x.CrossWalletBalance}, BalanceChange:{x.BalanceChange}}}"))};\n" +
			$"Positions: {string.Join(",", data.Positions.Select(x => $"{{Symbol:{x.Symbol}, Quantity:{x.Quantity}, UnrealizedPnl:{x.UnrealizedPnl}, RealizedPnl:{x.RealizedPnl}, EntryPrice:{x.EntryPrice}, PositionSide:{x.PositionSide}, MarginType:{x.MarginType}, IsolatedMargin:{x.IsolatedMargin}}}"))};");
	},
	order =>
	{
		var data = order.Data.UpdateData;
		Console.WriteLine($"\n\n\tOrderUpdate\n" +
			$"OrderId: {data.OrderId};\n" +
			$"Symbol: {data.Symbol};\n" +
			$"Side: {data.Side};\n" +
			$"Type: {data.Type};\n" +
			$"TimeInForce: {data.TimeInForce}\n" +
			$"Quantity: {data.Quantity}\n" +
			$"Price: {data.Price}\n" +
			$"AveragePrice: {data.AveragePrice}\n" +
			$"StopPrice: {data.StopPrice}\n" +
			$"ExecutionType: {data.ExecutionType}\n" +
			$"Status: {data.Status}\n" +
			$"QuantityOfLastFilledTrade: {data.QuantityOfLastFilledTrade}\n" +
			$"AccumulatedQuantityOfFilledTrades: {data.AccumulatedQuantityOfFilledTrades}\n" +
			$"PriceLastFilledTrade: {data.PriceLastFilledTrade}\n" +
			$"Fee: {data.Fee}\n" +
			$"FeeAsset: {data.FeeAsset}\n" +
			$"BidNotional: {data.BidNotional}\n" +
			$"AskNotional: {data.AskNotional}\n" +
			$"BuyerIsMaker: {data.BuyerIsMaker}\n" +
			$"IsReduce: {data.IsReduce}\n" +
			$"StopPriceWorking: {data.StopPriceWorking}\n" +
			$"OriginalType: {data.OriginalType}\n" +
			$"PositionSide: {data.PositionSide}\n" +
			$"PushedConditionalOrder: {data.PushedConditionalOrder}\n" +
			$"ActivationPrice: {data.ActivationPrice}\n" +
			$"CallbackRate: {data.CallbackRate}\n" +
			$"RealizedProfit: {data.RealizedProfit}");
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

string lastReadLine = "clear";
while(lastReadLine != "exit")
{
	if (lastReadLine == "clear")
	{
		Console.Clear();
	}

	lastReadLine = Console.ReadLine()!;
}

