using Ligric.Service.AuthService.Infrastructure.Persistence.Queries;

namespace Ligric.Service.AuthService.Application.CheckUserExists
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
