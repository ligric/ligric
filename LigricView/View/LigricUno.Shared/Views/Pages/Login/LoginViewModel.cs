using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Board;
using System;
using System.Collections.Generic;

namespace LigricUno.Views.Pages.Login
{
    public class LoginViewModel : DispatchedBindableBase
    {
        private string _login, _password;

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }


        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(LoginMethod));

        private readonly List<(Action<int> Action, int Index)> prerenderPageActions = new List<(Action<int> Action, int Index)>();

        private void LoginMethod(object parameter)
        {
            Navigation.GoTo(nameof(BoardPage));
        }
    }
}
