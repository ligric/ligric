using System.Threading;
using System.Threading.Tasks;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Infrastructure.Persistence.Commands;

namespace Ligric.Service.CryptoApisService.Infrastructure.Handlers.CreateUserApi
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
