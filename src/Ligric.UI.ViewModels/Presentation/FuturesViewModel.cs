using Ligric.Business.Apies;
using Ligric.Business.Futures;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturesViewModel
	{
		public FuturesViewModel(
			IApiesService apiesService,
			IOrdersService orderService)
		{
			Api = new ApisViewModel(apiesService, orderService);
			FuturesOrderPositions = new FutureOrdersViewModel(orderService);
			FuturePositions = new FuturePositionsViewModel();
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrderPositions { get; }

		public FuturePositionsViewModel FuturePositions { get; }

		
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
}
