using Ligric.Backend.Application.Configuration.Queries;

namespace Ligric.Backend.Application.Users.CheckUserExists
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
