using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Futures.Socket;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Sockets;

namespace Ligric.CryptoObserver;

public class BinanceFuturesManager
{
	private readonly BinanceApiAddresses _address;
	private readonly BinanceApiCredentials _credentials;
	private readonly BinanceClient _client;
	private readonly BinanceSocketClient _socketClient;

	//private Dictionary<>

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
		var startStreamResponse = await _client.UsdFuturesApi.Account.StartUserStreamAsync();
		var listenKey = startStreamResponse.Data ?? throw new ArgumentNullException();

		// TODO : Получаю список ордеров
		var ordersResponse = await _client.UsdFuturesApi.Trading.GetOpenOrdersAsync();
		IEnumerable<BinanceFuturesOrder> orders = ordersResponse.Data;

		await _socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync("BTCUSDT", OnAggregatedUpdated);

		await _socketClient.UsdFuturesStreams.SubscribeToUserDataUpdatesAsync(
			listenKey, null, null,
			OnAccountUpdated, OnOrdersUpdated, OnListenKeyExpired,
			null, null);
	}

	private void OnAggregatedUpdated(DataEvent<BinanceStreamAggregatedTrade> obj)
	{

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

	//private void OnPriceChanged(object sender, (string Symbol, decimal Price) e)
	//{
	//    //var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, e.Symbol));

	//    //if (entity != null)
	//    //{
	//    //    entity.Price = e.Price;
	//    //}
	//}

	//private async void OnOrderChanged1(object sender, (BinanceFuturesOrderDto Order, ActionCollectionEnum Action) e)
	//{
	//    //var order = e.Order;
	//    //WriteToDebug(order);

	//    //var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, order.Symbol));

	//    //if (entity != null && (order.Status == OrderStatus.Filled || order.Status == OrderStatus.Canceled))
	//    //{
	//    //    OrderViewModel removeOrder = entity.Orders.FirstOrDefault(x => string.Equals(x.ClientOrderId, order.ClientOrderId));
	//    //    if (removeOrder != null)
	//    //    {
	//    //        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
	//    //        {
	//    //            entity.Orders.Remove(removeOrder);
	//    //        });
	//    //    }
	//    //    return;
	//    //}

	//    //var newOrder = new OrderViewModel(order.ClientOrderId)
	//    //{
	//    //    Value = "Uknown",
	//    //    Side = order.Side.ToString(),
	//    //    Quantity = order.Quantity.ToString(),
	//    //    Price = order.Price.ToString(),
	//    //    Symbol = order.Symbol,
	//    //    Order = "Uknown"
	//    //};

	//    //if (entity == null && order.Status == OrderStatus.New)
	//    //{
	//    //    newFutureEntiry.Orders.AddAndRiseEvent(newOrder);

	//    //    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
	//    //    {
	//    //        //CurrentEntities.AddAndRiseEvent(newFutureEntiry);
	//    //    });
	//    //}

	//    //if (entity != null && order.Status == OrderStatus.New)
	//    //{
	//    //    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
	//    //    {
	//    //        entity.Orders.AddAndRiseEvent(newOrder);
	//    //    });
	//    //}
	//}

	//private void OnOpenOrdersChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<long, Ligric.Common.Types.Future.OpenOrderDto> e)
	//{
	//    throw new NotImplementedException();
	//}

	//private void OnFuturesChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<long, Ligric.Common.Types.Future.PositionDto> e)
	//{
	//    throw new NotImplementedException();
	//}

	//private void WriteToDebug(BinanceFuturesOrderDto dto)
	//{
	//    Debug.WriteLine(
	//        $"{nameof(dto.Pair)} {dto.Pair}\n" +
	//        $"{nameof(dto.Side)} {dto.Side}\n" +
	//        $"{nameof(dto.Status)} {dto.Status}\n" +
	//        $"{nameof(dto.Quantity)} {dto.Quantity}\n" +
	//        $"{nameof(dto.Symbol)} {dto.Symbol}\n" +
	//        $"{nameof(dto.Id)} {dto.Id}\n" +
	//        $"{nameof(dto.ClientOrderId)} {dto.ClientOrderId}" +
	//        $"\n {nameof(dto.AvgPrice)} {dto.AvgPrice} " +
	//        $"\n {nameof(dto.QuantityFilled)} {dto.QuantityFilled} " +
	//        $"\n {nameof(dto.QuoteQuantityFilled)} {dto.QuoteQuantityFilled}" +
	//        $"\n {nameof(dto.BaseQuantityFilled)} {dto.BaseQuantityFilled}" +
	//        $"\n {nameof(dto.LastFilledQuantity)} {dto.LastFilledQuantity}" +
	//        $"\n {nameof(dto.ReduceOnly)} {dto.ReduceOnly}" +
	//        $"\n {nameof(dto.ClosePosition)} {dto.ClosePosition}" +
	//        $"\n {nameof(dto.StopPrice)} {dto.StopPrice}" +
	//        $"\n {nameof(dto.TimeInForce)} {dto.TimeInForce}" +
	//        $"\n {nameof(dto.OriginalType)} {dto.OriginalType}" +
	//        $"\n {nameof(dto.Type)} {dto.Type}" +
	//        $"\n {nameof(dto.CallbackRate)} {dto.CallbackRate}" +
	//        $"\n {nameof(dto.ActivatePrice)} {dto.ActivatePrice}" +
	//        $"\n {nameof(dto.UpdateTime)} {dto.UpdateTime}" +
	//        $"\n {nameof(dto.CreateTime)} {dto.CreateTime}" +
	//        $"\n {nameof(dto.WorkingType)} {dto.WorkingType}" +
	//        $"\n {nameof(dto.PositionSide)} {dto.PositionSide}" +
	//        $"\n {nameof(dto.PriceProtect)} {dto.PriceProtect}");
	//}
}
