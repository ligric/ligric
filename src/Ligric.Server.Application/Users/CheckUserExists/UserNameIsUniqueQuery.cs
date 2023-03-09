using Ligric.Server.Application.Configuration.Queries;

namespace Ligric.Server.Application.Users.CheckUserExists
{
    public class UserNameIsUniqueQuery : IQuery<bool>
    {
        public string UserName { get; }

        public UserNameIsUniqueQuery(string username)
        {
            UserName = username;
        }
    }
}
