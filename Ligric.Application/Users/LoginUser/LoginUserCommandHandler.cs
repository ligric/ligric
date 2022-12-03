using Ligric.Application.Configuration.Commands;
using Ligric.Application.Users.LoginCustomer;
using Ligric.Common.Types;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Server.Domain.TypeExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ligric.Application.Users.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUniquenessChecker _userUniquenessChecker;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IUserUniquenessChecker userUniquenessChecker)
        {
            this._userRepository = userRepository;
            _userUniquenessChecker = userUniquenessChecker;
        }

        public Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            UserEntity user = this._userRepository.GetEntity(request.UserName);

            if (user == null) 
            {
                // TODO : Should be normal exception
                throw new ArgumentException("User not found.");
            }

            if (user.Password == request.Password)
            {
                return Task.FromResult(user.ToUserDto());
            }

            // TODO : Should be normal exception
            throw new ArgumentException("Wrong password.");
        }
    }
}