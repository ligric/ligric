using System.Threading;
using System.Threading.Tasks;
using Ligric.Service.AuthService.Domain.Checkers;
using Ligric.Service.AuthService.Infrastructure.Persistence.Queries;

namespace Ligric.Service.AuthService.UseCase.Handlers.CheckUserExists
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
