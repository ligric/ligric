using System.Collections.ObjectModel;

namespace LigricUno.Views.Pages.Profile
{

    public class CurrencyItem
    {
        public string Name { get; }

        public string FullName { get; }

        public decimal Rate { get; }

        public decimal Amount { get; }

        public CurrencyItem(string name, string fullName, decimal rate, decimal amount)
        {
            Name = name; FullName = fullName; Rate = rate; Amount = amount; 
        }
    }

    public class ProfilePageViewModel
    {
        public ObservableCollection<CurrencyItem> BinanceCurrencyItems { get; } = new ObservableCollection<CurrencyItem>()
        {
            new CurrencyItem("BTC", "Bitcoin", 0.00000126m, 4439.123m),
            new CurrencyItem("LUNA", "Terra", 0.0069m, 1734.15213m)
        };
    }
}
