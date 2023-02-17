using System;
using System.Linq;
using System.Reactive.Linq;
using Ligric.Domain.Types.Api;
using Ligric.Server.Domain.Entities.Apis;
using Ligric.Server.Domain.Entities.UserApies;
using Ligric.Server.Domain.TypeExtensions;
using Utils;

namespace Ligric.Application.UserApis
{
	public class UserApiObserver : IUserApiObserver
	{
		private readonly IApiRepository _apiRepository;
		private readonly IUserApiRepository _userApiRepository;

		private event Action<(EventAction, ApiClientDto)>? UserApiChanged;

		public UserApiObserver(
			IApiRepository apiRepository,
			IUserApiRepository repository)
		{
			_apiRepository = apiRepository;
			_userApiRepository = repository;
		}

		public void CreateUserApiFromOwner()
		{

		}

		public IObservable<(EventAction Action, ApiClientDto apiClient)> GetUserApiAsObservable(long userId)
		{
			var currentUserApies = _userApiRepository.GetEntitiesByUserId(userId).Select(x => (EventAction.Added, x.ToUserApiDto())).ToObservable();
			var updatedUserApies = Observable.FromEvent<(EventAction Action, ApiClientDto apiClient)>((x) => UserApiChanged += x, (x) => UserApiChanged -= x);

			if (currentUserApies != null && updatedUserApies != null)
			{
				return currentUserApies.Concat(updatedUserApies);
			}

			throw new NotImplementedException();
		}
	}
}
