using BoardsCommon.Enums;
using BoardsCore.Abstractions.BoardAbstractions.Absctacts;
using BoardsCore.Abstractions.BoardsAbstractions.Interfaces;
using BoardsCore.BitZlato;
using BoardsCore.BitZlato.Entities;
using Common.Enums;
using Common.EventArgs;
using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.RelayCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace LigricUno.Views.Pages.Board
{
    public class AdViewModel : DispatchedBindableBase
    {
        private long _id = 0;
        private string _trader, _paymentMethod, _rate, _limit;
        
        public long Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Trader { get => _trader; set => SetProperty(ref _trader, value); }
        public string PaymentMethod { get => _paymentMethod; set => SetProperty(ref _paymentMethod, value); }
        public string Rate { get => _rate; set => SetProperty(ref _rate, value); }
        public string Limit { get => _limit; set => SetProperty(ref _limit, value); }

        public AdViewModel(long id, string trader, string paymentMethod, string rate, string limit)
        {
            Id = id; Trader = trader; PaymentMethod = paymentMethod; Rate = rate; Limit = limit;
        }
    }

    public class BoardEntityViewModel : DispatchedBindableBase
    {
        public ObservableCollection<AdViewModel> Ads { get; } = new ObservableCollection<AdViewModel>();

        #region Private fields
        private long _id;
        private string _title;
        private Point _position;
        #endregion

        #region Properties
        public long Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public Point Position { get => _position; set => SetProperty(ref _position, value); }


        #region Test position
        private double _positionX, _positionY;


        public double PositionX { get => _positionX; set => SetProperty(ref _positionX, value); }
        public double PositionY { get => _positionY; set => SetProperty(ref _positionY, value); }


        #endregion

        #endregion

        #region Commands
        private RelayCommand<Point> _savePositionCommand;
        public RelayCommand<Point> SavePositionCommand => _savePositionCommand ?? (_savePositionCommand = new RelayCommand<Point>(async (e) => await OnSavePositionExecuteAsync(e)));        
        
        private RelayCommand<bool> _menuOptionSwitchCommand;
        public RelayCommand<bool> MenuOptionSwitchCommand => _menuOptionSwitchCommand ?? (_menuOptionSwitchCommand = new RelayCommand<bool>(OnMenuOptionSwitchExecuteAsync));

        private void OnMenuOptionSwitchExecuteAsync(bool e)
        {
            
        }

        private async Task OnSavePositionExecuteAsync(Point parameter)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("storage", CreationCollisionOption.OpenIfExists);
            File.WriteAllText(Path.Combine(folder.Path, "Settings.txt"), $"Board:{Id}\nX:{parameter.X }\nY:{parameter.Y}");
        }
        #endregion

        public BoardEntityViewModel(long id, string titel, double positionX = 0, double positionY = 0)
        {
            Id = id; Title = titel ?? "Uknown"; ; PositionX = positionX; PositionY = positionY;
        }
    }

    public class BitzlatoAdBoardViewModel : BoardEntityViewModel
    {
        #region IBitZlatoRequestsService initialization
        private static string apiKey = "{" +
                        "\"kty\":\"EC\"," +
                        "\"alg\":\"ES256\"," +
                        "\"crv\":\"P-256\"," +
                        "\"x\":\"WnVJnRzpTUo0mYEdkiDSuyGqfDZtBVLepkzqHk7O8SE\"," +
                        "\"y\":\"J9P-SkGy4qyyL6f-T9KHtJzwiTASHcxAxmwtWiUVF1Q\"," +
                        "\"d\":\"1xF-MartEnw4cAQB3eJC-Eg5YwThMemMx96DuHhyGFA\"" +
                        "}";

        private static string email = "balalay16@gmail.com";

        private static Dictionary<string, string> filters = new Dictionary<string, string>()
        {
            { "limit", "10" },
            { "currency", "RUB" },
            { "type", "purchase" },
            { "cryptocurrency", "BTC" }
        };
        #endregion

        private readonly AbstractBoardWithTimerNotifications<long, BitZlatoAdDto> model;
        public BitzlatoAdBoardViewModel(long id, string name, double positionX = 0, double positionY = 0) :
            base(id, name, positionX, positionY)
        {
            model = new BitZlatoBoardWithTimer(name, apiKey, email, TimeSpan.FromSeconds(5), filters, StateEnum.Stoped);

            foreach (var newValue in model.Ads.Values)
            {
                Ads.Add(new AdViewModel(newValue.Id, newValue.Trader.Name, newValue.Paymethod.Name, newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To, newValue.Rate.Value.ToString()));
            }
            //model.AdsChanged += Model_AdsChanged;
        }

        private void Model_AdsChanged(object sender, NotifyDictionaryChangedEventArgs<long, BitZlatoAdDto> e)
        {
            var newValue = e.NewValue;
            var oldValue = e.OldValue;

            switch (e.Action)
            {
                case NotifyDictionaryChangedAction.Added:
                    Ads.Add(new AdViewModel(newValue.Id, newValue.Trader.Name, newValue.Paymethod.Name, newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To, newValue.Rate.Value.ToString()));
                    break;
                case NotifyDictionaryChangedAction.Changed:
                    var index = Ads.IndexOf(Ads.FirstOrDefault(x => x.Id == newValue.Id));
                    Ads[index] = new AdViewModel(newValue.Id, newValue.Trader.Name, newValue.Paymethod.Name, newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To, newValue.Rate.Value.ToString());
                    break;
                case NotifyDictionaryChangedAction.Removed:
                    Ads.Remove(Ads.FirstOrDefault(x => x.Id == newValue.Id));
                    break;
                case NotifyDictionaryChangedAction.Cleared:
                    Ads.Clear();
                    break;
                case NotifyDictionaryChangedAction.Initialized:
                    Ads.Clear();

                    foreach (var item in e.NewDictionary.Values.Select(x => new AdViewModel(newValue.Id, newValue.Trader.Name, newValue.Paymethod.Name, newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To, newValue.Rate.Value.ToString())))
                    {
                        Ads.Add(item);
                    }
                    break;
            }
        }
    }

    public class BoardsViewModel : DispatchedBindableBase
    {
        private readonly IBoardsService _boardService;

        private static List<AdViewModel> testAds = new List<AdViewModel>()
        {
            new AdViewModel(0, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(1, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(2, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(3, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(4, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(5, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(6, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(7, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(8, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(8, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
            new AdViewModel(8, "Idrak", "Monobank", "10 000" + " - " + "100 000", "100 000 000"),
        };

        public ObservableCollection<BoardEntityViewModel> CurrentEntities { get; } = new ObservableCollection<BoardEntityViewModel>();

        private void OnAddBoardEntityExecute(BoardEntityType parameter)
        {
            switch (parameter)
            {
                case BoardEntityType.Ad:
                    var third = new BitzlatoAdBoardViewModel(1, "Popular BitZlato", 190, 120);

                    foreach (var item in testAds)
                        third.Ads.Add(item);

                    CurrentEntities.Add(third);
                    break;
                case BoardEntityType.Chart:
                    break;
                default:
                    break;
            }
        }

        public BoardsViewModel(IBoardsService boardsService)
        {
            _boardService = boardsService;

            var fist = new BitzlatoAdBoardViewModel(0, "BTC BitZlato", 10, 60);
            var second = new BitzlatoAdBoardViewModel(1, "ETH Binance", 280, 60);

            foreach (var item in testAds)
            {
                fist.Ads.Add(item);
                second.Ads.Add(item);
            }

            CurrentEntities.Add(fist);
            CurrentEntities.Add(second);
        }
    }
}
