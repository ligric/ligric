using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using Ligric.Business.Apies;
using Ligric.Business.Futures;
using Ligric.Domain.Types.Api;
using Ligric.UI.Infrastructure;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class OrderViewModel
	{
		[Reactive] public string? Symbol { get; set; }
		[Reactive] public string? Side { get; set; }
		[Reactive] public string? Quantity { get; set; }
		[Reactive] public string? Price { get; set; }
		[Reactive] public string? Order { get; set; }
		[Reactive] public string? Value { get; set; }
	}

	//public class PositionViewModel : DispatchedBindableBase
	//{
	//    private string _symbol, _side, _quantity, _openPrice, _currentPrice, _pnL, _pnLPercent;

	//    public string Symbol { get => _symbol; set => SetProperty(ref _symbol, value); }
	//    public string Side { get => _side; set => SetProperty(ref _side, value); }
	//    public string Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }
	//    public string OpenPrice { get => _openPrice; set => SetProperty(ref _openPrice, value); }
	//    public string CurrentPrice { get => _currentPrice; set => SetProperty(ref _currentPrice, value); }
	//    public string PnL { get => _pnL; set => SetProperty(ref _pnL, value); }
	//    public string PnLPercent { get => _pnLPercent; set => SetProperty(ref _pnLPercent, value); }
	//}

	public partial class ApisViewModel
	{
		private readonly IApiesService _apiService;
		private readonly IOrdersService _ordersService;
		public ApisViewModel(
			IApiesService apiesService,
			IOrdersService ordersService)
		{
			_apiService = apiesService;
			_ordersService = ordersService;

			ApisChangedEventSubscribe();
		}

		public ObservableCollection<ApiClientDto> Apis { get; init; } = new();

		[Reactive]
		public ApiDataViewModel AddingApi { get; } = new();

		public ReactiveCommand<Unit, Unit> SaveApiCommand => ReactiveCommand.CreateFromTask(
			execute: ct => ExecuteSaveApi(AddingApi, ct),
			canExecute: this.WhenAnyValue(x => x.AddingApi, api => CanSaveApi(api)));

		public ReactiveCommand<ApiClientDto, Unit> ShareApiCommand => ReactiveCommand.CreateFromTask<ApiClientDto>(
			execute: async (apiClient, ct) => await ExecuteShareApi(apiClient, ct));

		public ReactiveCommand<Unit, Unit> SelectAllCommand => ReactiveCommand.CreateFromTask(
			execute: ct => ExecuteSelectAll(ct));

		#region Save API
		private bool CanSaveApi(ApiDataViewModel api)
			=> api is { Name.Length: >= 1 } and { PrivateKey.Length: > 5 } and { PublicKey.Length: > 5 };

		private async Task ExecuteSaveApi(ApiDataViewModel api, CancellationToken ct)
		{
			if (api.Name != null && api.PublicKey != null && api.PrivateKey != null)
			{
				await _apiService.SaveApiAsync(new ApiDto(null, api.Name, api.PublicKey, api.PrivateKey), ct);
			}
		}
		#endregion

		private async Task ExecuteSelectAll(CancellationToken ct)
		{
			foreach (var api in Apis)
			{
				if (ct.IsCancellationRequested) break;

				await _ordersService.SubscribeOpenOrdersFromUserIdAsync(api.UserApiId ?? throw new ArgumentNullException("UserId is null"));
			}
		}

		public async Task ExecuteShareApi(ApiClientDto api, CancellationToken ct)
		{
			if (api != null)
			{
				await _apiService.ShareApiAsync(api, ct);
			}
		}

		#region Api Changed Subscribtion
		private void ApisChangedEventSubscribe()
		{
			var collectionChanged = Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(handler =>
			{
				NotifyCollectionChangedEventHandler collectionChanged = (sender, e) =>
				{
					handler(e);
				};

				return collectionChanged;
			}, fsHandler => _apiService.ApiesChanged += fsHandler, fsHandler => _apiService.ApiesChanged -= fsHandler);

			collectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnApiesChanged);
		}

		private void OnApiesChanged(NotifyCollectionChangedEventArgs e)
		{
			UpdateApisFromAction(e);
		}

		private async void UpdateApisFromAction(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					Apis.AddRange(e.NewItems);
					break;
			}
		}
		#endregion
	}

	public class ApiDataViewModel
	{
		[Reactive]
		public string? Name { get; set; } = "My test api key";

		[Reactive]
		public string? PublicKey { get; set; } = "c58577a8b8d83617fb678838fa8e43c83e53384e88fef416c81658e51c6c48f3";

		[Reactive]
		public string? PrivateKey { get; set; } = "651096d67c3d1a080daf6d26a37ad545864d312b7a6b24d5f654d4f26a1a7ddc";
	}

	public class FutureOrdersViewModel
	{
		private readonly IOrdersService _ordersService;
		public FutureOrdersViewModel(IOrdersService ordersService)
		{
			_ordersService = ordersService;

			_ordersService.OpenOrdersChanged += OnOpenOrdersChanged;
		}

		public ObservableCollection<OrderViewModel> OpenOrders { get; } = new ObservableCollection<OrderViewModel>();

		private void OnOpenOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, Domain.Types.Future.OpenOrderDto> e) => throw new NotImplementedException();
	}

	public class FuturesViewModel
	{
		public FuturesViewModel(
			IApiesService apiesService,
			IOrdersService orderService)
		{
			Api = new ApisViewModel(apiesService, orderService);
			FuturesOrderPositions = new FutureOrdersViewModel(orderService);
		}

		public ApisViewModel Api { get; }

		public FutureOrdersViewModel FuturesOrderPositions { get; }


		//private readonly IFuturesProvider _futuresProvider;
		//private readonly IApiesService _apiesService;
		//private RelayCommand _addNewApiCommand;

		//public FuturesViewModel(
		//    IFuturesProvider futuresProvider,
		//    IApiesService apiesService)
		//{
		//    _apiesService = apiesService;
		//    _futuresProvider = futuresProvider;

		//    _apiesService.ApiesChanged += OnApiesChanged;
		//    _futuresProvider.PositionsChanged += OnFuturesChanged;
		//    _futuresProvider.OpenOrdersChanged += OnOpenOrdersChanged;

		//    AddingApi.PropertyChanged += (s, e) =>
		//    {
		//        AddNewApiCommand.RaiseCanExecuteChanged();
		//    };
		//}

		//public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

		

		//public RelayCommand AddNewApiCommand => _addNewApiCommand ?? (
		//    _addNewApiCommand = new RelayCommand((async (e) => await OnAddNewApiExecuteAsync(e)), CanAddNewApiExecute));

		//private async Task OnAddNewApiExecuteAsync(object parameter)
		//{
		//    await _apiesService.SaveApiAsync(
		//        new Ligric.Common.Types.Api.ApiDto(
		//            null, 
		//            AddingApi.Name, 
		//            AddingApi.PublicKey, 
		//            AddingApi.PrivateKey)
		//        );

		//    AddingApi.Name = "";
		//    AddingApi.PublicKey = "";
		//    AddingApi.PrivateKey = "";
		//}

		//private void OnApiesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		//{
		//    throw new NotImplementedException();
		//}

		//private bool CanAddNewApiExecute(object parameter)
		//{
		//    if (string.IsNullOrEmpty(AddingApi?.PublicKey)
		//        || string.IsNullOrEmpty(AddingApi?.Name)
		//        || string.IsNullOrEmpty(AddingApi?.PrivateKey))
		//        return false;

		//    return true;
		//}

		//private async void OnPositionChanged(object sender, (BinanceFuturesPositionDto Position, ActionCollectionEnum Action) e)
		//{
		////    var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, e.Position.Symbol));

		////    if (e.Action == ActionCollectionEnum.Added)
		////    {
		////        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
		////        {
		////            entity.Positions.AddAndRiseEvent(new PositionViewModel
		////            {
		////                Symbol = e.Position.Symbol,
		////                CurrentPrice = e.Position.CurrentPrice,
		////                OpenPrice = e.Position.OpenPrice,
		////                PnL = e.Position.PnL,
		////                PnLPercent = e.Position.PnLPercent,
		////                Quantity = e.Position.Quantity,
		////                Side = e.Position.Side
		////            });
		////        });
		////    }
		////    else
		////    {
		////        var removePosition = entity.Positions.FirstOrDefault(x => string.Equals(x.Symbol, e.Position.Symbol) && string.Equals(x.Side, e.Position.Side));
		////        if (removePosition != null)
		////        {
		////            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
		////            {
		////                entity.Positions.Remove(removePosition);
		////            });
		////        }
		////    }
		//}

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
