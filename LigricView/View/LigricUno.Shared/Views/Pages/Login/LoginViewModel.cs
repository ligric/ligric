using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Board;
using LigricUno.Views.Pages.News;
using System;
using System.Collections.Generic;

namespace LigricUno.Views.Pages.Login
{
    public class LoginViewModel : DispatchedBindableBase
    {
        private string _email;

        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private RelayCommand _loginLaterCommand;
        public RelayCommand LoginLaterCommand => _loginLaterCommand ?? (_loginLaterCommand = new RelayCommand(LoginLaterMethod));

        private readonly List<(Action<int> Action, int Index)> prerenderPageActions = new List<(Action<int> Action, int Index)>();

        private void LoginLaterMethod(object parameter)
        {
            Navigation.GoTo(nameof(BoardPage));
        }
    }
}
