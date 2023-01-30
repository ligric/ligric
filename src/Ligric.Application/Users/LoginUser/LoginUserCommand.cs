using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Types.User;

namespace Ligric.Application.Users.LoginCustomer
{
    public class LoginUserCommand : CommandBase<UserDto>
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
