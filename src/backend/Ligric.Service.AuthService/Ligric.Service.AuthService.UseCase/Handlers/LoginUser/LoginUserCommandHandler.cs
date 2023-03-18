using Ligric.Service.AuthService.UseCase.Handlers.LoginCustomer;
using Ligric.Core.Types.User;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Infrastructure;
using Ligric.Service.AuthService.Infrastructure.Persistence.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ligric.Service.AuthService.UseCase.Handlers.LoginUser
{
	public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, UserResponseDto>
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

        public Task<UserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
			// TODO : Temporary realization
			var user = _userRepository.GetEntityByUserName(request.UserName);
			var hashedPass = _cryptoProvider.GetHash(request.Password, user?.Salt ?? string.Empty);

			if (string.Equals(user?.Password ?? string.Empty, hashedPass))
			{
				return Task.FromResult(new UserResponseDto(
					user?.Id ?? throw new ArgumentNullException($"{typeof(LoginUserCommandHandler)}: User Id is null"),
					user.UserName ?? throw new ArgumentNullException($"{typeof(LoginUserCommandHandler)}: User name is null")));
			}

			throw new NotImplementedException($"[{nameof(LoginUserCommandHandler)}] Some error");
		}
    }
}
