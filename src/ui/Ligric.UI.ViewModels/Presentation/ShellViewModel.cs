using Ligric.Business.Interfaces;

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

	private async void OnAuthorizationStateChanged(object? sender, Core.Types.User.UserAuthorizationState e)
	{
		switch (e)
		{
			case Core.Types.User.UserAuthorizationState.Connected:
				await _navigator.NavigateViewModelAsync<FuturesViewModel>(this);
				break;
			case Core.Types.User.UserAuthorizationState.Disconnected:
				await _navigator.GoBack(this);
				break;
		}
	}

	public virtual async Task Start()
	{
		await _navigator.NavigateViewModelAsync<AuthorizationViewModel>(this);
	}
}
