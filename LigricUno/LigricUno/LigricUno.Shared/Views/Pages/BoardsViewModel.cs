using LigricMvvm.BaseMvvm;
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
        private string _title = "BitZlato";


        public int Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private ObservableCollection<AdViewModel> _ads = new ObservableCollection<AdViewModel>()
        {
            new AdViewModel(){ Trader = "limeniye",  PaymentMethod = "Альфа Банк", Rate = "9 999 999" , Limit = "100 000 - 140 130" }
        };

        public ObservableCollection<AdViewModel> Ads { get => _ads; set => SetProperty(ref _ads, value); }
    }

    public class BoardsViewModel : OnNotifyPropertyChanged
    {
        private BoardViewModel _testBoard = new BoardViewModel();
        public BoardViewModel TestBoard { get => _testBoard; set => SetProperty(ref _testBoard, value); }

        public ObservableCollection<BoardViewModel> Boards = new ObservableCollection<BoardViewModel>();
    }
}
