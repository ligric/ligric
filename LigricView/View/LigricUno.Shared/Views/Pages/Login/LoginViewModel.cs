using Grpc.Net.Client;
using Ligric.Business;
using Ligric.Common.Abstractions;
using Ligric.Common.Types;
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
        private readonly IAuthorizationService _authorizationService;

        private string _login, _password;

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public LoginViewModel()
        {
            GrpcChannel grpcChannel = GrpcChannelHalper.GetGrpcChannel();
            var metadataRepos = new MetadataRepository();

            _authorizationService = new AuthorizationService(grpcChannel, metadataRepos);
            _authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
        }

        private void OnAuthorizationStateChanged(object sender, Ligric.Common.Types.UserAuthorizationState e)
        {
            switch(e)
            {
                case UserAuthorizationState.Connected:
                    Navigation.GoTo(nameof(BoardPage));
                    break;
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(LoginMethod));

        private async void LoginMethod(object parameter)
        {
            await _authorizationService.SignInAsync(Login, Password);
        }
    }
}
