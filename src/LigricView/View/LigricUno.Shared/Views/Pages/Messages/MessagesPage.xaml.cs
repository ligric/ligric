using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace LigricUno.Views.Pages.Messages
{
    public class NameToFirsSymbolsConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Join(" ", value.ToString().Split(' ').Select(x => x[0]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed partial class MessagesPage : Page
    {
        public MessagesPage()
        {
            this.InitializeComponent();
        }
    }
}
