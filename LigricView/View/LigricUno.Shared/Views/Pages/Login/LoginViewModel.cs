using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.News;

namespace LigricUno.Views.Pages.Login
{
    public class LoginViewModel : OnNotifyPropertyChanged
    {
        private string _email;

        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private RelayCommand _loginLaterCommand;
        public RelayCommand LoginLaterCommand => _loginLaterCommand ?? (_loginLaterCommand = new RelayCommand(LoginLaterMethod));

        private void LoginLaterMethod(object parameter)
        {
            Navigation.GoTo(new NewsPage(), nameof(NewsPage), new NewsViewModel());
        }
    }
}
