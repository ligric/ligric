using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Types.User;

namespace Ligric.Backend.Application.Users.LoginCustomer
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
