using BoardModels.AbstractBoardNotifications.Abstractions;
using BoardModels.BitZlato;
using BoardModels.BitZlato.Entities;
using BoardModels.CommonTypes.Entities;
using Common.Enums;
using Common.EventArgs;
using LigricMvvmToolkit.BaseMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LigricUno.Views.Pages
{
    public class AdViewModel : OnNotifyPropertyChanged
    {
        private long _id = 0;
        private string _trader, _paymentMethod, _rate, _limit;
        
        public long Id { get => _id; set => SetProperty(ref _id, value); }
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

        private AbstractBoardWithTimerNotifications<BitZlatoAdDto> model = new BitZlatoBoardWithTimer("BitZlato", apiKey, email, TimeSpan.FromSeconds(5), filters, StateEnum.Active);

        public BitzlatoBoardViewModel()
        {
            Title = model.Name;
            foreach (var newValue in model.Ads.Values)
            {
                Ads.Add(new AdViewModel()
                {
                    Id = newValue.Id,
                    Trader = newValue.Trader.Name,
                    PaymentMethod = newValue.Paymethod.Name,
                    Limit = newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To,
                    Rate = newValue.Rate.Value.ToString()
                });
            }
            model.AdsChanged += Model_AdsChanged;
        }

        private void Model_AdsChanged(object sender, NotifyDictionaryChangedEventArgs<long, BitZlatoAdDto> e)
        {
            var newValue = e.NewValue;
            var oldValue = e.OldValue;

            switch (e.Action)
            {
                case NotifyDictionaryChangedAction.Added:
                    Ads.Add(new AdViewModel()
                    {
                        Id = newValue.Id,
                        Trader = newValue.Trader.Name,
                        PaymentMethod = newValue.Paymethod.Name,
                        Limit = newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To,
                        Rate = newValue.Rate.Value.ToString()
                    });
                    break;
                case NotifyDictionaryChangedAction.Changed:
                    var index = Ads.IndexOf(Ads.FirstOrDefault(x => x.Id == newValue.Id));
                    Ads[index] = new AdViewModel()
                                 {
                                     Id = newValue.Id,
                                     Trader = newValue.Trader.Name,
                                     PaymentMethod = newValue.Paymethod.Name,
                                     Limit = newValue.LimitCurrencyRight.From + " - " + newValue.LimitCurrencyRight.To,
                                     Rate = newValue.Rate.Value.ToString()
                                 };
                    break;
                case NotifyDictionaryChangedAction.Removed:
                    Ads.Remove(Ads.FirstOrDefault(x => x.Id == newValue.Id));
                    break;
                case NotifyDictionaryChangedAction.Cleared:
                    Ads.Clear();
                    break;
            }
        }
    }

    public class BoardsViewModel : OnNotifyPropertyChanged
    {
        private BitzlatoBoardViewModel _testBoard = new BitzlatoBoardViewModel();
        public BitzlatoBoardViewModel TestBoard { get => _testBoard; set => SetProperty(ref _testBoard, value); }


        public ObservableCollection<BoardViewModel> Boards = new ObservableCollection<BoardViewModel>();
    }
}
