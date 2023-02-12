using Ligric.Domain.Types.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ligric.Business.Authorization;

public interface IAuthorizationService : IDisposable
{
	UserDto CurrentUser { get; }

	UserAuthorizationState CurrentConnectionState { get; }

	event EventHandler<UserAuthorizationState> AuthorizationStateChanged;

	Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct);

	bool IsUserNameUnique(string userName);

	void SignUp(string userName, string password);

	Task SignUpAsync(string userName, string password, CancellationToken ct);

	void SignIn(string userName, string password);

	Task SignInAsync(string userName, string password, CancellationToken ct);
}
