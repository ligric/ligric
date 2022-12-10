using DevPace.Core.DataTypes;
using System;
using System.Threading.Tasks;

namespace DevPace.Core;

public interface IAuthorizationService : IDisposable
{
    UserDto CurrentUser { get; }

    UserAuthorizationState CurrentConnectionState { get; }

    event EventHandler<UserAuthorizationState> AuthorizationStateChanged;

    Task<bool> IsLoginUniqueAsync(string login);

    bool IsLoginUnique(string login);

    Task<bool> IsEmailUniqueAsync(string login);

    bool IsEmailUnique(string login);

    void SignUp(string login, string password, string email);

    Task SignUpAsync(string login, string password, string email);
}
