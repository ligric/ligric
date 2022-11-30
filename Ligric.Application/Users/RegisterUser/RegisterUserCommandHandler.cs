using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Users;
using Ligric.Domain.SeedWork;
using System;

namespace Ligric.Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUniqueIdGenerator _idGenerator;
        private readonly IUserUniquenessChecker _userUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IUniqueIdGenerator idGenerator,
            IUserUniquenessChecker userUniquenessChecker, 
            IUnitOfWork unitOfWork)
        {
            this._userRepository = userRepository;
            this._idGenerator = idGenerator;
            _userUniquenessChecker = userUniquenessChecker;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //var user = User.CreateRegistered(request.Login, request.Password, request.Email, this._idGenerator, this._userUniquenessChecker);

            //await this._userRepository.AddAsync(user);

            //await this._unitOfWork.CommitAsync(cancellationToken);

            //return new UserDto(user.Id.Value, user.Login);

            return null;
        }
    }
}