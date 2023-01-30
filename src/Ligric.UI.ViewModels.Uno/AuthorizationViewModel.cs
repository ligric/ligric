using Ligric.Business;
using Ligric.Domain.Client.Base;
using Ligric.Domain.Client.Base.Futures;
using Ligric.Domain.Types.User;
using Microsoft.UI.Xaml.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Threading.Tasks;

namespace Ligric.UI.ViewModels.Uno
{
    public class AuthorizationViewModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Frame _frame;

        public AuthorizationViewModel(IAuthorizationService service, Frame frame)
        {
            _frame = frame;
            _authorizationService = service;
            _authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
        }

        [Reactive] public string Login { get; set; }

        [Reactive] public string Password { get; set; }


        public ReactiveCommand<Unit, Unit> LoginCommand => ReactiveCommand.CreateFromTask(LoginMethod);

        private async Task LoginMethod()
        {
            await _authorizationService.SignInAsync(Login, Password);
        }

        private void OnAuthorizationStateChanged(object sender, UserAuthorizationState e)
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
