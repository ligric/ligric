using System.Threading;
using System.Threading.Tasks;
using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Backend.Domain.Entities.Apies;
using Ligric.Backend.Domain.Entities.Apis;

namespace Ligric.Backend.Application.UserApis.CreateUserApi
{
	public class CreateUserApiCommandHandler : ICommandHandler<CreateUserApiCommand, long>
	{
		private readonly IApiRepository _apiRepository;
		private readonly IUserApiObserver _userApiObserver;

		public CreateUserApiCommandHandler(
			IApiRepository apiRepository,
			IUserApiObserver userApiObserver)
		{
			_userApiObserver = userApiObserver;
			_apiRepository = apiRepository;
		}

		public async Task<long> Handle(CreateUserApiCommand request, CancellationToken cancellationToken)
		{
			// #Should be canvensions

			long apiId = (long)_apiRepository.Save(new ApiEntity
			{
				PrivateKey = request.PrivateKey,
				PublicKey = request.PublicKey,
			});

			long userApiId = _userApiObserver.Save(apiId, request.Name, request.OwnerId, request.Permissions);

			return userApiId;
		}
	}
}
