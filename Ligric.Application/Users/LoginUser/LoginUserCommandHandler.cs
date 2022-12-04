using Ligric.Application.Configuration.Commands;
using Ligric.Application.Providers.Security;
using Ligric.Application.Users.LoginCustomer;
using Ligric.Common.Types;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Server.Domain.TypeExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils.Cryptography;

namespace Ligric.Application.Users.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IUserUniquenessChecker _userUniquenessChecker;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            ICryptoProvider cryptoProvider,
            IUserUniquenessChecker userUniquenessChecker)
        {
            this._userRepository = userRepository;
            this._cryptoProvider = cryptoProvider;
            this._userUniquenessChecker = userUniquenessChecker;
        }

        public Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var salt = _cryptoProvider.GetSalt(request.UserName);
            var passwordHashed = _cryptoProvider.GetHash(request.Password, salt);

            UserEntity user = this._userRepository.GetEntity(request.UserName);

            if (user == null) 
            {
                // TODO : Should be normal exception
                throw new ArgumentException("User not found.");
            }

            if (string.Equals(user.Password, passwordHashed))
            {
                return Task.FromResult(user.ToUserDto());
            }

            // TODO : Should be normal exception
            throw new ArgumentException("Wrong password.");
        }
    }
}