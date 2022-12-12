using Ligric.Common.Types.User;
using System;
using System.Threading.Tasks;

namespace Ligric.Domain.Client.Base;

public interface IAuthorizationService : IDisposable
{
    UserDto CurrentUser { get; }

    UserAuthorizationState CurrentConnectionState { get; }

    event EventHandler<UserAuthorizationState> AuthorizationStateChanged;

    Task<bool> IsUserNameUniqueAsync(string userName);

    bool IsUserNameUnique(string userName);

    void SignUp(string userName, string password);

    Task SignUpAsync(string userName, string password);

    void SignIn(string userName, string password);

    Task SignInAsync(string userName, string password);
}
