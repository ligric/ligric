﻿using Utils;
using System.Reactive.Linq;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Extensions;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;

namespace Ligric.Service.CryptoApisService.Application.Observers.UserApi
{
	public class UserApiObserver : IUserApiObserver
	{
		private readonly IUserApiRepository _userApiRepository;
		private event Action<(EventAction Action, long UserId, ApiClientResponseDto userApi)>? ApiChanged;

		public UserApiObserver(IUserApiRepository userApiRepository)
		{
			_userApiRepository = userApiRepository;
		}

		/// <returns>UserApiId</returns>
		public long Save(long apiId, string apiName, long userId, int permissions)
		{
			var userApiSaveEntity = new UserApiEntity
			{
				ApiId = apiId,
				UserId = userId,
				Name = apiName,
				Permissions = permissions
			};

			var userApiId = (long)_userApiRepository.Save(userApiSaveEntity);

			userApiSaveEntity.Id = userApiId;
			ApiChanged?.Invoke((EventAction.Added, userId, userApiSaveEntity.ToApiClientResponseDto()));

			return userApiId;
		}

		public void ShareToAll(long userApiId, int permissions)
		{
			var userApi = _userApiRepository.GetEntityById(userApiId);

			if (userApi == null)
			{
				throw new ArgumentNullException($"UserApi with id {userApiId} not found");
			}
			// TODO : TEMPORARY
			// REASON: because of deadline
			var userIds = _userApiRepository.GetUserIdsThatDontHaveTheseApi(userApiId);

			foreach (var userId in userIds)
			{
				Save(userApi.ApiId ?? throw new NullReferenceException("[UserApi] api id is null here"),
					userApi.Name ?? "Api title",
					userId,
					permissions);
			}
		}

		public long Share(long userApiId, long sharedUserId, int permissions)
		{
			var userApiSaveEntity = new UserApiEntity
			{
				UserId = sharedUserId,
				Permissions = permissions
			};

			var userApi = _userApiRepository.GetEntityById(userApiId);
			if (userApi == null)
			{
				throw new ArgumentException($"GetEntityById from userId {userApiId} was null");
			}
			userApiSaveEntity.ApiId = userApi.ApiId;

			var newUserApiId = (long)_userApiRepository.Save(userApiSaveEntity);
			userApiSaveEntity.Id = newUserApiId;

			ApiChanged?.Invoke((EventAction.Added, sharedUserId, userApiSaveEntity.ToApiClientResponseDto()));

			return userApiId;
		}

		public IObservable<(EventAction Action, long UserId, ApiClientResponseDto Api)> GetApisAsObservable(long userId)
		{
			var userApiEntities = _userApiRepository.GetAllowedApiInfoByUserId(userId);
			var currentApiStateNotifications = userApiEntities.Select(x => (EventAction.Added, userId, x)).ToObservable();
			var updatedApiStateNotifications = Observable.FromEvent<(EventAction Action, long UserId, ApiClientResponseDto Api)>(
				(x) => ApiChanged += x, (x) => ApiChanged -= x);

			return currentApiStateNotifications.Concat(updatedApiStateNotifications);
		}
	}
}
