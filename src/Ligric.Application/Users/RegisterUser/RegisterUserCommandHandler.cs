using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Server.Domain.SeedWork;
using Ligric.Domain.Types.User;
using System;
using Ligric.Application.Providers.Security;
using Ligric.Application.Users.LoginUser;

namespace Ligric.Application.Users.RegisterUser
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

			var user = _userRepository.Save(new UserEntity
			{
				UserName = request.Login,
				Password = hashedPass,
				Salt = salt
			}) as UserEntity;

			return new UserDto(
					user?.Id ?? throw new ArgumentNullException($"{typeof(RegisterUserCommandHandler)}: User Id is null"),
					user?.UserName ?? throw new ArgumentNullException($"{typeof(RegisterUserCommandHandler)}: User Name is null"));
        }
    }
}
