using Ligric.Business;
using Ligric.Domain.Client.Base.Futures;
using Ligric.Domain.Types.User;

namespace Ligric.UI.Infrastructure.Presentation
{
    public partial class AuthorizationViewModel
    {
        //private readonly IAuthorizationService _authorizationService;
        private readonly INavigator _navigator;

		
        public AuthorizationViewModel(INavigator navigator)
        {
            _navigator = navigator;
            //_authorizationService = service;
            //_authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
        }

        public string? Login { get; set; }

        public string? Password { get; set; }


        //public ReactiveCommand<Unit, Unit> LoginCommand => ReactiveCommand.CreateFromTask(LoginMethod);

        public async ValueTask SignIn(CancellationToken ct)
        {
            //var user = await _authService.AuthenticateAsync(_dispatcher);

            //if (user is not null)
            if (true)
            {
                await _navigator.NavigateViewModelAsync<FuturesViewModel>(this);
                //await _navigator.NavigateRouteAsync(this, string.Empty, cancellation: ct);
            }
        }

        private async Task LoginMethod()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                throw new System.ArgumentNullException("Login or Password is null");
            }

            //await _authorizationService.SignInAsync(Login, Password);
        }

        private void OnAuthorizationStateChanged(object? sender, UserAuthorizationState e)
        {
            switch (e)
            {
                case UserAuthorizationState.Connected:
                    IFuturesProvider futures = new FuturesProvider();
                    //_frame.Navigate(typeof(AuthorizationPage));
                    //Application.nav
                    //Navigation.GoTo(new FuturesPage(), nameof(FuturesPage), futures);
                    break;
            }
        }
    }
}
