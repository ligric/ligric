using Ligric.Business;
using Ligric.Domain.Client.Base;
using Ligric.Domain.Client.Base.Futures;
using Ligric.Domain.Types.User;

namespace Ligric.UI.Infrastructure.Presentation
{
    public class AuthorizationViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationViewModel(IAuthorizationService service)
        {
            _authorizationService = service;
            _authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
        }

        public string? Login { get; set; }

        public string? Password { get; set; }


        //public ReactiveCommand<Unit, Unit> LoginCommand => ReactiveCommand.CreateFromTask(LoginMethod);

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
