namespace Ligric.UI.ViewModels.Uno
{
    public class AuthorizationViewModel
    {
        //public static readonly IAuthorizationService _authorizationService;
        //public static readonly IMetadataRepository _metadataRepository;

        //private string _login, _password;

        //public string Login { get => _login; set => SetProperty(ref _login, value); }
        //public string Password { get => _password; set => SetProperty(ref _password, value); }

        //static LoginViewModel()
        //{
        //    GrpcChannel grpcChannel = GrpcChannelHalper.GetGrpcChannel();
        //    _metadataRepository = new MetadataRepository();

        //    _authorizationService = new AuthorizationService(grpcChannel, _metadataRepository);
        //    _authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
        //}

        //public LoginViewModel()
        //{

        //}

        //private static void OnAuthorizationStateChanged(object sender, UserAuthorizationState e)
        //{
        //    switch (e)
        //    {
        //        case UserAuthorizationState.Connected:
        //            IFuturesProvider futures = new FuturesProvider();
        //            Navigation.GoTo(new FuturesPage(), nameof(FuturesPage), futures);
        //            break;
        //    }
        //}

        //private RelayCommand _loginCommand;
        //public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(LoginMethod));

        //private async void LoginMethod(object parameter)
        //{
        //    await _authorizationService.SignInAsync(Login, Password);
        //}
    }
}
