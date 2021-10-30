using Windows.UI.Xaml.Controls;

namespace LigricUno.Views.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private bool isLoaded = false;
        private void OnEmailTextBoxGettingFocus(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            if (!isLoaded)
            {
                isLoaded = true;
                args.TryCancel();
                return;
            }
        }
    }
}
