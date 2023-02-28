using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Futures.Socket;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Sockets;
using Ligric.CryptoObserver.Extensions;
using Ligric.Domain.Types.Future;

namespace Ligric.CryptoObserver;

public class BinanceFuturesManager
{
	private readonly BinanceApiAddresses _address;
	private readonly BinanceApiCredentials _credentials;
	private readonly BinanceClient _client;
	private readonly BinanceSocketClient _socketClient;

	private CancellationTokenSource? _ordersSubscribeCancellationToken;

	private List<FuturesOrderDto> orders = new List<FuturesOrderDto>();
	private List<FuturesPositionDto> positions = new List<FuturesPositionDto>();



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

	public async Task AttachOrdersSubscribtionsAsync()
	{
		if (_ordersSubscribeCancellationToken != null
				&& !_ordersSubscribeCancellationToken.IsCancellationRequested)
		{
			return;
		}
#pragma warning disable CS8602 // Dereference of a possibly null reference.
		var token = _ordersSubscribeCancellationToken.Token;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

		var startStreamResponse = await _client.UsdFuturesApi.Account.StartUserStreamAsync(token);
		var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

		var updateSubscription = await _socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
			listenKey, null, null,
			OnAccountUpdated, OnOrdersUpdated, OnListenKeyExpired,
			null, null);





		var ordersResponse = await _client.UsdFuturesApi.Trading.GetOpenOrdersAsync(ct: token);
		orders = ordersResponse.Data.Select(binanceOrder => binanceOrder.ToFuturesOrderDto()).ToList();

		foreach (var order in orders)
		{

		}

		foreach (var order in orders)
		{
			await _socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(order.Symbol, OnAggregatedUpdated);
		}


	}

	private void OnAggregatedUpdated(DataEvent<BinanceStreamAggregatedTrade> obj)
	{
		//var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, e.Symbol));

		//if (entity != null)
		//{
		//    entity.Price = e.Price;
		//}
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
		BinanceFuturesStreamOrderUpdateData streamOrder = order.Data.UpdateData;
		FuturesOrderDto orderDto = streamOrder.ToFuturesOrderDto();


		{
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

		{
			//var order = e.Order;
			//WriteToDebug(order);

			//var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, order.Symbol));

			//if (entity != null && (order.Status == OrderStatus.Filled || order.Status == OrderStatus.Canceled))
			//{
			//    OrderViewModel removeOrder = entity.Orders.FirstOrDefault(x => string.Equals(x.ClientOrderId, order.ClientOrderId));
			//    if (removeOrder != null)
			//    {
			//        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			//        {
			//            entity.Orders.Remove(removeOrder);
			//        });
			//    }
			//    return;
			//}

			//var newOrder = new OrderViewModel(order.ClientOrderId)
			//{
			//    Value = "Uknown",
			//    Side = order.Side.ToString(),
			//    Quantity = order.Quantity.ToString(),
			//    Price = order.Price.ToString(),
			//    Symbol = order.Symbol,
			//    Order = "Uknown"
			//};

			//if (entity == null && order.Status == OrderStatus.New)
			//{
			//    newFutureEntiry.Orders.AddAndRiseEvent(newOrder);

			//    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			//    {
			//        //CurrentEntities.AddAndRiseEvent(newFutureEntiry);
			//    });
			//}

			//if (entity != null && order.Status == OrderStatus.New)
			//{
			//    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			//    {
			//        entity.Orders.AddAndRiseEvent(newOrder);
			//    });
			//}
		}
	}

	private void OnListenKeyExpired(DataEvent<BinanceStreamEvent> obj)
	{
		System.Diagnostics.Debug.WriteLine("Listen key expired");
	}
}
