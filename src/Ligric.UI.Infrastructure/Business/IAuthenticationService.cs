namespace Ligric.UI.Infrastructure.Business.Data;

public interface IAuthenticationService: IAuthenticationTokenProvider
{
	Task<UserContext?> GetCurrentUserAsync();

	Task<UserContext?> AuthenticateAsync(IDispatcher dispatcher);

	Task SignOutAsync();
}
