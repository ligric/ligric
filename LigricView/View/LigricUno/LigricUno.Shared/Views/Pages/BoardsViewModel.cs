using BoardModels.AbstractBoardNotifications.Abstractions;
using BoardModels.BitZlato;
using BoardModels.BitZlato.Entities;
using BoardModels.CommonTypes.Entities;
using LigricMvvmToolkit.BaseMvvm;
using System.Collections.ObjectModel;

namespace LigricUno.Views.Pages
{
    public class AdViewModel : OnNotifyPropertyChanged
    {
        private int _id = 0;
        private string _trader, _paymentMethod, _rate, _limit;
        
        public int Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Trader { get => _trader; set => SetProperty(ref _trader, value); }
        public string PaymentMethod { get => _paymentMethod; set => SetProperty(ref _paymentMethod, value); }
        public string Rate { get => _rate; set => SetProperty(ref _rate, value); }
        public string Limit { get => _limit; set => SetProperty(ref _limit, value); }
    }


    public class BoardViewModel : OnNotifyPropertyChanged
    {
        private int _id = 0;
        private string _title;

        public int Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private ObservableCollection<AdViewModel> _ads = new ObservableCollection<AdViewModel>();

        public ObservableCollection<AdViewModel> Ads { get => _ads; set => SetProperty(ref _ads, value); }
    }


    public class BitzlatoBoardViewModel : BoardViewModel
    {
        private AbstractBoardWithTimerNotifications<AdDto> model = new BitZlatoBoardWithTimer<BitZlatoAdDto>();
    }

    public class BoardsViewModel : OnNotifyPropertyChanged
    {
        private BitzlatoBoardViewModel _testBoard = new BitzlatoBoardViewModel();


        public BoardViewModel TestBoard { get => _testBoard; set => SetProperty(ref _testBoard, value); }

        public ObservableCollection<BoardViewModel> Boards = new ObservableCollection<BoardViewModel>();
    }
}
