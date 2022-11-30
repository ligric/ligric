using Ligric.Application.Configuration.Queries;

namespace Ligric.Application.Users.CheckUserExists
{
    public class LoginIsUniqueQuery : IQuery<bool>
    {
        public string Login { get; }

        public LoginIsUniqueQuery(string login)
        {
            Login = login;
        }
    }
}
