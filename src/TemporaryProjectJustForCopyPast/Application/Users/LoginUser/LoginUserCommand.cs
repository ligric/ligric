using Ligric.Application.Configuration.Commands;
using Ligric.Core.Types.User;

namespace TemporaryProjectJustForCopyPast.Application.Users.LoginUser
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
