
namespace Ligric.UI.Infrastructure.Presentation;

public class ShellViewModel
{
    private readonly INavigator _navigator;
    //private readonly IAuthenticationTokenProvider _auth;

    public ShellViewModel(
        INavigator navigator
        /*IAuthenticationTokenProvider authentication*/)
    {
        _navigator = navigator;
        //_auth = authentication;

        _ = Start();
    }

    public async Task Start()
    {
        //var token = await _auth.GetAccessToken();
        //if (string.IsNullOrWhiteSpace(token))
        if (true)
        {
            //await _navigator.NavigateViewModelAsync<WelcomeViewModel>(this);
            await _navigator.NavigateViewModelAsync<AuthorizationViewModel>(this);
        }
        else
        {
            //await _navigator.NavigateViewModelAsync<AuthorizationViewModel>(this);
        }
    }
}
