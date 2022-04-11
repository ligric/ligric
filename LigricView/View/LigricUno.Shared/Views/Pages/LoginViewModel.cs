using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using Windows.UI.Xaml.Controls;

namespace LigricUno.Views.Pages
{
    public class LoginViewModel : OnNotifyPropertyChanged
    {
        private string _email;

        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private RelayCommand _loginLaterCommand;
        public RelayCommand LoginLaterCommand => _loginLaterCommand ?? (_loginLaterCommand = new RelayCommand(LoginLaterMethod));

        private void LoginLaterMethod(object parameter)
        {
            var firstPageKey = nameof(NewsPage);

            Navigation.PrerenderPage(new LoginPage() { Tag = firstPageKey, /*Background = new SolidColorBrush(Colors.Blue)*/ },
                firstPageKey, new LoginViewModel());
            Navigation.GoTo(firstPageKey);
        }
    }
}
