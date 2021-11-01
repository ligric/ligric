using LigricMvvm.BaseMvvm;
using LigricMvvm.Navigation;
using LigricMvvm.RelayCommand;
using System.Threading.Tasks;

namespace LigricUno.Views.Pages
{
    public class LoginViewModel : OnNotifyPropertyChanged
    {
        private string _email;

        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private RelayCommand _loginLaterCommand;
        public RelayCommand LoginLaterCommand => _loginLaterCommand ?? (_loginLaterCommand = new RelayCommand(LoginLaterMethod));

        private async void LoginLaterMethod(object parameter)
        {
            await Navigation.GoTo(nameof(HomePage), new HomePage());
        }
    }
}
