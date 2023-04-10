using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Application.TemporaryObservers
{
	public class UserApiObserver : IUserApiObserver
	{
		//private readonly IUserApiRepository _userApiRepository;
		//private readonly IUserRepository _userRepository;
		//private event Action<(EventAction Action, long UserId, ApiClientDto userApi)>? ApiChanged;

		public UserApiObserver(
			//IUserApiRepository userApiRepository,
			//IUserRepository userRepository
			)
		{
			//_userApiRepository = userApiRepository;
			//_userRepository = userRepository;
		}

		/// <returns>UserApiId</returns>
		public long Save(long apiId, string apiName, long userId, int permissions)
		{
			UserApiEntity userApiSaveEntity = new UserApiEntity
			{
				ApiId = apiId,
				UserId = userId,
				Name = apiName,
				Permissions = permissions
			};

			//long userApiId = (long)_userApiRepository.Save(userApiSaveEntity);

			//userApiSaveEntity.Id = userApiId;

			//ApiChanged?.Invoke((EventAction.Added, userId, userApiSaveEntity.ToUserApiDto()));

			//return userApiId;
			return -1;
		}

		public void ShareToAll(long userApiId, int permissions)
		{
			//var userApi = _userApiRepository.GetEntityById(userApiId);

			//if (userApi != null)
			//{
			//	var userIds = _userRepository.GetUserIdsThatDontHaveTheseApi(userApiId);

			//	foreach (var userId in userIds)
			//	{
			//		Save(userApi.ApiId ?? throw new NullReferenceException("[UserApi] api id is null here"),
			//			userApi.Name ?? "Api title",
			//			userId,
			//			permissions);
			//	}
			//}
			//else
			//{
			//	throw new ArgumentNullException($"UserApi with id {userApiId} not found");
			//}
		}

		public long Share(long userApiId, long sharedUserId, int permissions)
		{
			//UserApiEntity userApiSaveEntity = new UserApiEntity
			//{
			//	UserId = sharedUserId,
			//	Permissions = permissions
			//};

			//var userApi = _userApiRepository.GetEntityById(userApiId);
			//if (userApi == null)
			//{
			//	throw new ArgumentException($"GetEntityById from userId {userApiId} was null");
			//}

			//   userApiSaveEntity.ApiId = userApi.ApiId;


			//long newUserApiId = (long)_userApiRepository.Save(userApiSaveEntity);
			//userApiSaveEntity.Id = newUserApiId;

			//ApiChanged?.Invoke((EventAction.Added, sharedUserId, userApiSaveEntity.ToUserApiDto()));

			//return userApiId;
			return -1;
		}

		//public IObservable<(EventAction Action, long UserId, ApiClientDto Api)> GetApisAsObservable(long userId)
		//{
		//	var userApiEntities = _userApiRepository.GetAllowedApiInfoByUserId(userId);
		//	var currentApiStateNotifications = userApiEntities.Select(x => (EventAction.Added, userId, x)).ToObservable();
		//	var updatedApiStateNotifications = Observable.FromEvent<(EventAction Action, long UserId, ApiClientDto Api)>((x) => ApiChanged += x, (x) => ApiChanged -= x);

		//	return currentApiStateNotifications.Concat(updatedApiStateNotifications);
		//}
	}
}
