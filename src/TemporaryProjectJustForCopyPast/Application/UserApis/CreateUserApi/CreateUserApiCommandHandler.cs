using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Entities.Apies;
using Ligric.Domain.Entities.Apis;

namespace TemporaryProjectJustForCopyPast.Application.UserApis.CreateUserApi
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

			var apiId = (long)_apiRepository.Save(new ApiEntity
			{
				PrivateKey = request.PrivateKey,
				PublicKey = request.PublicKey,
			});

			var userApiId = _userApiObserver.Save(apiId, request.Name, request.OwnerId, request.Permissions);

			return userApiId;
		}
	}
}
