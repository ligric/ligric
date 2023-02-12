using Ligric.Business.Authorization;

namespace Ligric.UI.ViewModels.Presentation;

public class ShellViewModel
{
	private readonly INavigator _navigator;
	private readonly IAuthorizationService _authorizationService;

	public ShellViewModel(
		INavigator navigator,
		IAuthorizationService authorizationService)
	{
		_navigator = navigator;
		_authorizationService = authorizationService;
		_authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
		_ = Start();
	}

	private async void OnAuthorizationStateChanged(object sender, Domain.Types.User.UserAuthorizationState e)
	{
		switch (e)
		{
			case Domain.Types.User.UserAuthorizationState.Connected:
				await _navigator.NavigateViewModelAsync<FuturesViewModel>(this);
				break;
			case Domain.Types.User.UserAuthorizationState.Disconnected:
				await _navigator.NavigateViewModelAsync<AuthorizationViewModel>(this);
				break;
		}
	}

	public virtual async Task Start()
	{
		await _navigator.NavigateViewModelAsync<AuthorizationViewModel>(this);
	}
}
