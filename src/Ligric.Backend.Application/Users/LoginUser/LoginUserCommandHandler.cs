using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Backend.Application.Providers.Security;
using Ligric.Backend.Application.Users.LoginCustomer;
using Ligric.Domain.Types.User;
using Ligric.Backend.Domain.Entities.Users;
using Ligric.Backend.Domain.TypeExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ligric.Backend.Application.Users.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoProvider _cryptoProvider;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            ICryptoProvider cryptoProvider)
        {
            this._userRepository = userRepository;
            this._cryptoProvider = cryptoProvider;
        }

        public Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
			// TODO : Temporary realization
			var user = _userRepository.GetEntity(request.UserName);
			var hashedPass = _cryptoProvider.GetHash(request.Password, user?.Salt ?? string.Empty);

			if (string.Equals(user?.Password ?? string.Empty, hashedPass))
			{
				return Task.FromResult(new UserDto(
					user?.Id ?? throw new ArgumentNullException($"{typeof(LoginUserCommandHandler)}: User Id is null"),
					request.UserName));
			}

			throw new NotImplementedException("Some error");
		}
    }
}
