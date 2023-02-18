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

			long apiId = (long)_apiRepository.Save(new ApiEntity
			{
				Name = request.Name,
				PrivateKey = request.PrivateKey,
				PublicKey = request.PublicKey,
			});

			long userApiId = (long)_userApiRepository.Save(new UserApiEntity
			{
				ApiId = apiId,
				UserId = request.OwnerId,
				Permissions = request.Permissions
			});

			var notification = new ApiClientNotification(userApiId, request.Name, request.Permissions);
			await _mediator.Publish(notification);

			return userApiId;
		}

		public async Task Handle(ApiClientNotification notification, CancellationToken cancellationToken)
		{

		}
	}
}
