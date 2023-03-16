using Ligric.Core.Types.User;
using Ligric.Service.AuthService.Infrastructure.Persistence.Commands;

namespace Ligric.Service.AuthService.UseCase.Handlers.RegisterUser
{
    public class RegisterUserCommand : CommandBase<UserResponseDto>
    {
        public string Login { get; }      

        public string Password { get; }

        public RegisterUserCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
