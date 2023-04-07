using Ligric.Core.Types.User;
using Ligric.Service.AuthService.Infrastructure.Persistence.Commands;

namespace Ligric.Service.AuthService.UseCase.Handlers.LoginCustomer
{
    public class LoginUserCommand : CommandBase<UserResponseDto>
    {
        public string UserName { get; }

        public string Password { get; }

        public LoginUserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
