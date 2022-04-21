using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Boards;
using LigricUno.Views.Pages.News;
using LigricUno.Views.Pages.Profile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

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
            Navigation.GoTo(new BoardsPage(), nameof(BoardsPage) + 0, new BoardsViewModel());
        }
    }
}
