using Ligric.UI.ViewModels.Uno;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Ligric.UI.Uno.Pages
{
    public sealed partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            this.InitializeComponent();
            DataContext = new AuthorizationViewModel();
        }

        private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FuturesPage));
        }
    }
}
