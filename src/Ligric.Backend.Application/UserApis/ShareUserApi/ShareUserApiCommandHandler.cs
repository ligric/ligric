using System.Threading;
using System.Threading.Tasks;
using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Backend.Domain.Entities.Apis;
using Ligric.Backend.Domain.Entities.Users;
using MediatR;

namespace Ligric.Backend.Application.UserApis.ShareUserApi
{
	public class ShareUserApiCommandHandler : ICommandHandler<ShareUserApiCommand, bool>
	{
		private readonly IUserRepository _userRepository;
		private readonly IApiRepository _apiRepository;
		private readonly IUserApiObserver _userApiObserver;

		public ShareUserApiCommandHandler(
			IUserRepository userRepository,
			IApiRepository apiRepository,
			IUserApiObserver userApiObserver)
		{
			_userRepository = userRepository;
			_userApiObserver = userApiObserver;
			_apiRepository = apiRepository;
		}

		public async Task<bool> Handle(ShareUserApiCommand request, CancellationToken cancellationToken)
		{
			if (request.UserIds.Count > 0)
			{
				//foreach (var userId in request.UserIds)
				//{
				//	_userApiObserver.Share(request.UserApiId, userId, request.Permissions);
				//}
			}
			else
			{
				_userApiObserver.ShareToAll(request.UserApiId, request.Permissions);
			}

			return true;
		}
	}
}
