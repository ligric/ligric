using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;

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

            Navigation.PrerenderPage(new NewsPage() { Tag = firstPageKey, /*Background = new SolidColorBrush(Colors.Blue)*/ },
                firstPageKey, new NewsViewModel());

            Navigation.GoTo(firstPageKey);
        }
    }
}
