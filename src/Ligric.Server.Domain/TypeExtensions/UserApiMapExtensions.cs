using Ligric.Domain.Types.Api;
using Ligric.Server.Domain.Entities.UserApies;

namespace Ligric.Server.Domain.TypeExtensions
{
	public static class UserApiMapExtensions
	{
		public static ApiClientDto ToUserApiDto(this UserApiEntity userApi)
		{
			return new ApiClientDto(userApi.Id, userApi?.Name ?? "Not titled", userApi?.Permissions ?? 0);
		}

		public static UserApiEntity ToUserApiEntity(this ApiClientDto userApi)
		{
			return new UserApiEntity
			{
				Id = userApi.UserApiId,
				Name= userApi.Name,
				Permissions = userApi.Permissions
			};
		}
	}
}
