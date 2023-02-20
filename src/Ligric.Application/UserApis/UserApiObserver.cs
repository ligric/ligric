using System;
using System.Linq;
using System.Reactive.Linq;
using Ligric.Domain.Types.Api;
using Ligric.Server.Domain.Entities.UserApies;
using Ligric.Server.Domain.TypeExtensions;
using MediatR;
using Utils;

namespace Ligric.Application.UserApis
{
	public class UserApiObserver : IUserApiObserver
	{
		private readonly IUserApiRepository _userApiRepository;
		private event Action<(EventAction Action, ApiClientDto userApi)>? ApiChanged;

		public UserApiObserver(IUserApiRepository userApiRepository)
		{
			_userApiRepository = userApiRepository;
		}

		/// <returns>UserApiId</returns>
		public long Save(long apiId, long userId, int permissions)
		{
			UserApiEntity userApiSaveEntity = new UserApiEntity
			{
				ApiId = apiId,
				UserId = userId,
				Permissions = permissions
			};

			long userApiId = (long)_userApiRepository.Save(userApiSaveEntity);

			userApiSaveEntity.Id = userApiId;

			ApiChanged?.Invoke((EventAction.Added, userApiSaveEntity.ToUserApiDto()));

			return userApiId;
		}

		public IObservable<(EventAction Action, ApiClientDto Api)> GetApisAsObservable(long userId)
		{
			var userApiEntities = _userApiRepository.GetEntitiesByUserId(userId);
			var currentApiStateNotifications = userApiEntities.Select(x => (EventAction.Added, x.ToUserApiDto())).ToObservable();
			var updatedApiStateNotifications = Observable.FromEvent<(EventAction Action, ApiClientDto Api)>((x) => ApiChanged += x, (x) => ApiChanged -= x);

			return currentApiStateNotifications.Concat(updatedApiStateNotifications);
		}
	}
}
