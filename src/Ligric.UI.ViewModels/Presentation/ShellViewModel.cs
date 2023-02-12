namespace Ligric.UI.ViewModels.Presentation;

public class ShellViewModel
{
	private readonly INavigator _navigator;

	public ShellViewModel(INavigator navigator)
	{
		_navigator = navigator;

		_ = Start();
	}

	public virtual async Task Start()
	{
		await _navigator.NavigateViewModelAsync<AuthorizationViewModel>(this);
	}
}
