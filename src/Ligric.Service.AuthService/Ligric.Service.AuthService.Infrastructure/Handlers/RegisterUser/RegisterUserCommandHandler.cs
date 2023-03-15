using System.Threading;
using System.Threading.Tasks;
using Ligric.Core.Types.User;
using System;
using Ligric.Service.AuthService.Infrastructure.Persistence.Commands;
using Ligric.Service.AuthService.Infrastructure;
using Ligric.Service.AuthService.Domain.Entities;
using Ligric.Service.AuthService.Application.Repositories;

namespace Ligric.Application.Users.RegisterUser
{
	public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserResponseDto>
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

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
			var salt = _cryptoProvider.GetSalt(Guid.NewGuid().ToString());
			var hashedPass = _cryptoProvider.GetHash(request.Password, salt);

			var userId = (long)_userRepository.Save(new UserEntity
			{
				UserName = request.Login,
				Password = hashedPass,
				Salt = salt
			});

			return new UserResponseDto(userId, request.Login);
        }
    }
}
