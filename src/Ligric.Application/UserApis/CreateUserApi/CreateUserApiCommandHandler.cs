using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Types.Api;
using Ligric.Server.Domain.Entities.Apies;
using Ligric.Server.Domain.Entities.Apis;
using Ligric.Server.Domain.Entities.UserApies;
using MediatR;

namespace Ligric.Application.UserApis.CreateUserApi
{
	public class ApiClientNotification : ApiClientDto, INotification
	{
		public ApiClientNotification(long id, string name, int apiPermissions)
			: base(id, name, apiPermissions)
		{
		}
	}

	public class CreateUserApiCommandHandler : ICommandHandler<CreateUserApiCommand, long>, INotificationHandler<ApiClientNotification>
	{
		private readonly IMediator _mediator;
		private readonly IApiRepository _apiRepository;
		private readonly IUserApiRepository _userApiRepository;

		public CreateUserApiCommandHandler(
			IMediator mediator,
			IApiRepository apiRepository,
			IUserApiRepository userApiRepository)
		{
			_mediator = mediator;
			_apiRepository = apiRepository;
			_userApiRepository = userApiRepository;

		}

		public async Task<long> Handle(CreateUserApiCommand request, CancellationToken cancellationToken)
		{
			// #Should be canvensions

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

			var notification = new ApiClientNotification(userApiEntity?.Id ?? throw new System.Exception(), "", 1);
			await _mediator.Publish(notification);

			return userApiEntity?.Id ?? throw new System.Exception();
		}

		public async Task Handle(ApiClientNotification notification, CancellationToken cancellationToken)
		{

		}
	}
}
