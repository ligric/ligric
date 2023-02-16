using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Server.Domain.Entities.Apies;
using Ligric.Server.Domain.Entities.Apis;
using Ligric.Server.Domain.Entities.UserApies;

namespace Ligric.Application.UserApis.CreateUserApi
{
	public class CreateUserApiCommandHandler : ICommandHandler<CreateUserApiCommand, long>
	{
		private readonly IApiRepository _apiRepository;
		private readonly IUserApiRepository _userApiRepository;

		public CreateUserApiCommandHandler(
			IApiRepository apiRepository,
			IUserApiRepository userApiRepository)
		{
			_apiRepository = apiRepository;
			_userApiRepository = userApiRepository;

		}

		public async Task<long> Handle(CreateUserApiCommand request, CancellationToken cancellationToken)
		{
			// Should be long protection
			var apiEntity = (ApiEntity)_apiRepository.Save(new ApiEntity
			{
				Name = request.Name,
				PrivateKey = request.PrivateKey,
				PublicKey = request.PublicKey,
			});

			var userApiEntity = (UserApiEntity)_userApiRepository.Save(new UserApiEntity
			{
				ApiId = apiEntity.Id,
				UserId = request.OwnerId,
				Permissions = request.Permissions
			});

			return userApiEntity?.Id ?? throw new System.Exception();
		}
	}
}
