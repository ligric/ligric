using Ligric.Service.AuthService.Infrastructure.Persistence.Queries;

namespace Ligric.Service.AuthService.UseCase.Handlers.CheckUserExists
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
