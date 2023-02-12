using System.Windows.Input;
using Ligric.UI.Infrastructure.Models;
using Uno.Extensions.Reactive;

namespace Ligric.UI.ViewModels.Presentation
{
	public partial class AuthorizationViewModel
    {
        private readonly INavigator _navigator;

        public AuthorizationViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

		public IState<Credentials> Credentials => State<Credentials>.Empty(this);

		public ICommand SignIn => Command.Create(b => b.Given(Credentials).When(CanSignIn).Then(DoSignIn));

		private bool CanSignIn(Credentials credentials)
		=> credentials is { UserName.Length: > 0 } and { Password.Length: > 0 };

		private async ValueTask DoSignIn(Credentials credentials, CancellationToken ct)
		{
			//await _navigator.NavigateViewModelAsync<FuturesViewModel>(this);
		}
	}
}
