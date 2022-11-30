using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Queries;
using Ligric.Domain.Users;

namespace Ligric.Application.Users.CheckUserExists
{
    public class LoginIsUniqueQueryHandler : IQueryHandler<LoginIsUniqueQuery, bool>
    {
        private readonly IUserUniquenessChecker _userUniquenessChecker;

        public LoginIsUniqueQueryHandler(
            IUserUniquenessChecker userUniquenessChecker)
        {
            this._userUniquenessChecker = userUniquenessChecker;
        }

        public Task<bool> Handle(LoginIsUniqueQuery request, CancellationToken cancellationToken)
        {
            bool isLoginUnique = this._userUniquenessChecker.IsLoginUnique(request.Login);
            return Task.FromResult(isLoginUnique);
        }
    }
}