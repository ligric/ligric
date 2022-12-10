using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricUno.Views.Pages.Profile
{
    public class TreeViewItemsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HeaderItemTemplate { get; set; }
        public DataTemplate ContentItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return MySelectTemplate(item);
        }

        private DataTemplate MySelectTemplate(object value)
        {
            if (value is CurrencyItem currencyItem)
                return ContentItemTemplate;

            if (value is FinancialExchange financialExchange)
                return HeaderItemTemplate;

            throw new System.NotImplementedException($"Uknown DataTemplate type. \nType: {nameof(TreeViewItemsDataTemplateSelector)}\nType: {value}");
        }
    }

    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }
    }
}
