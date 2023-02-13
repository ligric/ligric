using System.Windows.Input;
using Ligric.Business.Authorization;
using Ligric.UI.ViewModels.Data;
using Uno.Extensions.Reactive;

namespace Ligric.UI.ViewModels.Presentation
{
	public partial class AuthorizationViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationViewModel(IAuthorizationService authorizationService)
        {
			_authorizationService = authorizationService;
		}

		public IState<AuthorizationCredentials> Credentials => State<AuthorizationCredentials>.Empty(this);

		public ICommand AuthorizateCommand => Command.Create(b => b.Given(Credentials).When(CanAuthorizate).Then(DoAuthorizate));

		// TODO : Temporary string parameter
		public async ValueTask SetAuthorizationMode(string mode, CancellationToken ct)
		{
			AutorizationMode modeEnum = (AutorizationMode)Enum.Parse(typeof(AutorizationMode), mode);

			await Credentials.UpdateValue(opt => opt.Map(credintials => credintials with { AutorizationMode = modeEnum }), ct);
		}

		private bool CanAuthorizate(AuthorizationCredentials credentials)
		{
			if (credentials.UserName?.Length > 0 && credentials.Password?.Length > 5)
			{
				return false;
			}

			if (credentials.AutorizationMode == AutorizationMode.SignUp && string.Equals(credentials.Password, credentials.RepeatedPassword))
			{
				return true;
			}

			return false;
		}

		private async ValueTask DoAuthorizate(AuthorizationCredentials credentials, CancellationToken ct)
		{
			if (credentials.UserName == null || credentials.Password == null)
			{
				throw new NotImplementedException();
			}

			if (credentials.AutorizationMode == AutorizationMode.SignIn)
			{
				await _authorizationService.SignInAsync(credentials.UserName, credentials.Password, ct);
			}
			else
			{
				await _authorizationService.SignUpAsync(credentials.UserName, credentials.Password, ct);
			}
		}
	}
}
