using System.Threading;
using System.Threading.Tasks;
using Ligric.Server.Application.Configuration.Queries;
using Ligric.Server.Domain.Entities.Users;

namespace Ligric.Server.Application.Users.CheckUserExists
{
    public class UserNameIsUniqueQueryHandler : IQueryHandler<UserNameIsUniqueQuery, bool>
    {
        private readonly IUserUniquenessChecker _userUniquenessChecker;

        public UserNameIsUniqueQueryHandler(
            IUserUniquenessChecker userUniquenessChecker)
        {
            this._userUniquenessChecker = userUniquenessChecker;
        }

        public Task<bool> Handle(UserNameIsUniqueQuery request, CancellationToken cancellationToken)
        {
            bool isLoginUnique = this._userUniquenessChecker.IsLoginUnique(request.UserName);
            return Task.FromResult(isLoginUnique);
        }
    }
}