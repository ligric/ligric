using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ligric.UI.ViewModels.Uno
{
    //public class OrderViewModel : DispatchedBindableBase
    //{
    //    private string _symbol, _side, _quantity, _price, _order, _value;

    //    public OrderViewModel(string clientOrderId)
    //    {
    //        ClientOrderId = clientOrderId;
    //    }

    //    public string ClientOrderId { get; }

    //    public string Symbol { get => _symbol; set => SetProperty(ref _symbol, value); }
    //    public string Side { get => _side; set => SetProperty(ref _side, value); }
    //    public string Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }
    //    public string Price { get => _price; set => SetProperty(ref _price, value); }
    //    public string Order { get => _order; set => SetProperty(ref _order, value); }
    //    public string Value { get => _value; set => SetProperty(ref _value, value); }
    //}

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

    //public class ApiKeyViewModel : DispatchedBindableBase
    //{
    //    private string _name, _publicKey, _privateKey;

    //    public string Name { get => _name; set => SetProperty(ref _name, value); }
    //    public string PublicKey { get => _publicKey; set => SetProperty(ref _publicKey, value); }
    //    public string PrivateKey { get => _privateKey; set => SetProperty(ref _privateKey, value); }
    //}

    public class FuturesViewModel
    {
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

        //public ApiKeyViewModel AddingApi { get; } = new ApiKeyViewModel();

        //public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

        //public ObservableCollection<OrderViewModel> OpenOrders { get; } = new ObservableCollection<OrderViewModel>();

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
        ////            entity.Positions.Add(new PositionViewModel
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
        //    //    newFutureEntiry.Orders.Add(newOrder);

        //    //    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    //    {
        //    //        //CurrentEntities.Add(newFutureEntiry);
        //    //    });
        //    //}

        //    //if (entity != null && order.Status == OrderStatus.New)
        //    //{
        //    //    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    //    {
        //    //        entity.Orders.Add(newOrder);
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
