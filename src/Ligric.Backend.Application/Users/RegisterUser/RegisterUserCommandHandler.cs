using System.Threading;
using System.Threading.Tasks;
using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Backend.Domain.Entities.Users;
using Ligric.Backend.Domain.SeedWork;
using Ligric.Domain.Types.User;
using System;
using Ligric.Backend.Application.Providers.Security;
using Ligric.Backend.Application.Users.LoginUser;

namespace Ligric.Backend.Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
		private readonly ICryptoProvider _cryptoProvider;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
			ICryptoProvider cryptoProvider)
        {
            _userRepository = userRepository;
			_cryptoProvider = cryptoProvider;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
			var salt = _cryptoProvider.GetSalt(Guid.NewGuid().ToString());
			var hashedPass = _cryptoProvider.GetHash(request.Password, salt);

			var userId = (long)_userRepository.Save(new UserEntity
			{
				UserName = request.Login,
				Password = hashedPass,
				Salt = salt
			});

			return new UserDto(userId, request.Login);
        }
    }
}
