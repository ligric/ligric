using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Server.Domain.Entities.Apies;
using Ligric.Server.Domain.Entities.Apis;

namespace Ligric.Application.UserApis.CreateUserApi
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
				Name = request.Name,
				PrivateKey = request.PrivateKey,
				PublicKey = request.PublicKey,
			});

			long userApiId = _userApiObserver.Save(apiId, request.OwnerId, request.Permissions);

			return userApiId;
		}
	}
}
