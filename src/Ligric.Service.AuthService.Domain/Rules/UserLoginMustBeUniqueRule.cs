using Ligric.Domain.SeedWork;
using Ligric.Service.AuthService.Domain.Checkers;

namespace Ligric.Service.AuthService.Domain.Rules
{
    public class UserLoginMustBeUniqueRule : IBusinessRule
    {
        private readonly IUserUniquenessChecker _userUniquenessChecker;

        private readonly string _login;

        public UserLoginMustBeUniqueRule(
            IUserUniquenessChecker userUniquenessChecker, 
            string login)
        {
            _userUniquenessChecker = userUniquenessChecker;
            _login = login;
        }

        public bool IsBroken() => !_userUniquenessChecker.IsLoginUnique(_login);
			
        public string Message => "User with this login already exists.";
    }
}
