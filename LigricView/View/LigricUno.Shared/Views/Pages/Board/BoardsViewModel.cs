using Binance.Net.Enums;
using Binance.Net.Ligric.Business;
using Common.Enums;
using Ligric.Common.Types;
using LigricMvvmToolkit.BaseMvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;

namespace LigricUno.Views.Pages.Board
{

    public class OrderViewModel : DispatchedBindableBase
    {
        private string _symbol, _side, _quantity, _price, _order, _value;

        public OrderViewModel(string clientOrderId)
        {
            ClientOrderId = clientOrderId;
        }

        public string ClientOrderId { get; }

        public string Symbol { get => _symbol; set => SetProperty(ref _symbol, value); }
        public string Side { get => _side; set => SetProperty(ref _side, value); }
        public string Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }
        public string Price { get => _price; set => SetProperty(ref _price, value); }
        public string Order { get => _order; set => SetProperty(ref _order, value); }
        public string Value { get => _value; set => SetProperty(ref _value, value); }
    }

    public class PositionViewModel : DispatchedBindableBase
    {
        private string _symbol, _side, _quantity, _openPrice, _currentPrice, _pnL, _pnLPercent;

        public string Symbol { get => _symbol; set => SetProperty(ref _symbol, value); }
        public string Side { get => _side; set => SetProperty(ref _side, value); }
        public string Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }
        public string OpenPrice { get => _openPrice; set => SetProperty(ref _openPrice, value); }
        public string CurrentPrice { get => _currentPrice; set => SetProperty(ref _currentPrice, value); }
        public string PnL { get => _pnL; set => SetProperty(ref _pnL, value); }
        public string PnLPercent { get => _pnLPercent; set => SetProperty(ref _pnLPercent, value); }
    }

    public class FutureEntityViewModel : DispatchedBindableBase
    {
        //public ObservableCollection<AdViewModel> Orders { get; } = new ObservableCollection<AdViewModel>();

        #region Private fields
        private double _positionX, _positionY;
        private decimal _price;

        private string _symbol;
        private Point _position;
        #endregion

        public ObservableCollection<OrderViewModel> Orders { get; } = new ObservableCollection<OrderViewModel>();
        public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

        public FutureEntityViewModel(string symbol, double positionX = 0, double positionY = 0)
        {
            Symbol = symbol ?? "Uknown"; PositionX = positionX; PositionY = positionY;
        }

        #region Properties
        public string Symbol { get => _symbol; set => SetProperty(ref _symbol, value); }
        public decimal Price { get => _price; set => SetProperty(ref _price, value); }
        public Point Position { get => _position; set => SetProperty(ref _position, value); }
        public double PositionX { get => _positionX; set => SetProperty(ref _positionX, value); }
        public double PositionY { get => _positionY; set => SetProperty(ref _positionY, value); }

        #endregion
    }

    public class BoardsViewModel : DispatchedBindableBase
    {
        public BoardsViewModel(ApiDto api)
        {
            Class1 class1 = new Class1(
                "c58577a8b8d83617fb678838fa8e43c83e53384e88fef416c81658e51c6c48f3",
                "651096d67c3d1a080daf6d26a37ad545864d312b7a6b24d5f654d4f26a1a7ddc");

            class1.OrderChanged += OnOrderChanged1;

            class1.PriceChanged += OnPriceChanged;

            class1.PositionChanged += OnPositionChanged;
        }

        private async void OnPositionChanged(object sender, (BinanceFuturesPositionDto Position, ActionCollectionEnum Action) e)
        {
            var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, e.Position.Symbol));

            if (e.Action == ActionCollectionEnum.Added)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    entity.Positions.Add(new PositionViewModel
                    {
                        Symbol = e.Position.Symbol,
                        CurrentPrice = e.Position.CurrentPrice,
                        OpenPrice = e.Position.OpenPrice,
                        PnL = e.Position.PnL,
                        PnLPercent = e.Position.PnLPercent,
                        Quantity = e.Position.Quantity,
                        Side = e.Position.Side
                    });
                });
            }
            else
            {
                var removePosition = entity.Positions.FirstOrDefault(x => string.Equals(x.Symbol, e.Position.Symbol) && string.Equals(x.Side, e.Position.Side));
                if (removePosition != null)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        entity.Positions.Remove(removePosition);
                    });
                }
            }
        }

        public ObservableCollection<FutureEntityViewModel> CurrentEntities { get; } = new ObservableCollection<FutureEntityViewModel>();

        private void OnPriceChanged(object sender, (string Symbol, decimal Price) e)
        {
            var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, e.Symbol));

            if (entity != null)
            {
                entity.Price = e.Price;
            }
        }

        private async void OnOrderChanged1(object sender, (BinanceFuturesOrderDto Order, ActionCollectionEnum Action) e)
        {
            var order = e.Order;
            WriteToDebug(order);

            var entity = CurrentEntities.FirstOrDefault(x => string.Equals(x.Symbol, order.Symbol));

            if (entity != null && (order.Status == OrderStatus.Filled || order.Status == OrderStatus.Canceled))
            {
                OrderViewModel removeOrder = entity.Orders.FirstOrDefault(x => string.Equals(x.ClientOrderId, order.ClientOrderId));
                if (removeOrder != null)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        entity.Orders.Remove(removeOrder);
                    });
                }
                return;
            }

            var newOrder = new OrderViewModel(order.ClientOrderId)
            {
                Value = "Uknown",
                Side = order.Side.ToString(),
                Quantity = order.Quantity.ToString(),
                Price = order.Price.ToString(),
                Symbol = order.Symbol,
                Order = "Uknown"
            };

            if (entity == null && order.Status == OrderStatus.New)
            {
                var newFutureEntiry = new FutureEntityViewModel(order.Symbol)
                {
                    PositionX = 100,
                    PositionY = 100
                };

                newFutureEntiry.Orders.Add(newOrder);

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    CurrentEntities.Add(newFutureEntiry);
                });
            }

            if (entity != null && order.Status == OrderStatus.New)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    entity.Orders.Add(newOrder);
                });
            }
        }

        private void WriteToDebug(BinanceFuturesOrderDto dto)
        {
            Debug.WriteLine(
                $"{nameof(dto.Pair)} {dto.Pair}\n" +
                $"{nameof(dto.Side)} {dto.Side}\n" +
                $"{nameof(dto.Status)} {dto.Status}\n" +
                $"{nameof(dto.Quantity)} {dto.Quantity}\n" +
                $"{nameof(dto.Symbol)} {dto.Symbol}\n" +
                $"{nameof(dto.Id)} {dto.Id}\n" +
                $"{nameof(dto.ClientOrderId)} {dto.ClientOrderId}" +
                $"\n {nameof(dto.AvgPrice)} {dto.AvgPrice} " +
                $"\n {nameof(dto.QuantityFilled)} {dto.QuantityFilled} " +
                $"\n {nameof(dto.QuoteQuantityFilled)} {dto.QuoteQuantityFilled}" +
                $"\n {nameof(dto.BaseQuantityFilled)} {dto.BaseQuantityFilled}" +
                $"\n {nameof(dto.LastFilledQuantity)} {dto.LastFilledQuantity}" +
                $"\n {nameof(dto.ReduceOnly)} {dto.ReduceOnly}" +
                $"\n {nameof(dto.ClosePosition)} {dto.ClosePosition}" +
                $"\n {nameof(dto.StopPrice)} {dto.StopPrice}" +
                $"\n {nameof(dto.TimeInForce)} {dto.TimeInForce}" +
                $"\n {nameof(dto.OriginalType)} {dto.OriginalType}" +
                $"\n {nameof(dto.Type)} {dto.Type}" +
                $"\n {nameof(dto.CallbackRate)} {dto.CallbackRate}" +
                $"\n {nameof(dto.ActivatePrice)} {dto.ActivatePrice}" +
                $"\n {nameof(dto.UpdateTime)} {dto.UpdateTime}" +
                $"\n {nameof(dto.CreateTime)} {dto.CreateTime}" +
                $"\n {nameof(dto.WorkingType)} {dto.WorkingType}" +
                $"\n {nameof(dto.PositionSide)} {dto.PositionSide}" +
                $"\n {nameof(dto.PriceProtect)} {dto.PriceProtect}");
        }
    }
}
