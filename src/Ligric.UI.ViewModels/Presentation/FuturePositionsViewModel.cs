using System.Collections.ObjectModel;
using Ligric.Business.Futures;
using Ligric.UI.ViewModels.Data;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FuturePositionsViewModel
	{
		private readonly IOrdersService _ordersService;
		private readonly IValuesService _valuesService;
		private readonly IPositionsService _postionsService;

		public ObservableCollection<PositionViewModel> Positions { get; } = new ObservableCollection<PositionViewModel>();

		internal FuturePositionsViewModel(
			IOrdersService ordersService,
			IValuesService valuesService,
			IPositionsService postionsService)
		{
			_ordersService = ordersService;
			_valuesService = valuesService;
			_postionsService = postionsService;	
		}
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

	}
}
