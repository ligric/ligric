using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;

namespace Ligric.Domain.TypeExtensions
{
	public static class UserApiMapExtensions
	{
		public static ApiClientResponseDto ToApiClientResponseDto(this UserApiEntity userApi)
		{
			return new ApiClientResponseDto(
				userApi.UserId ?? throw new System.ArgumentNullException("UserId is null"),
				userApi?.Name ?? "Not titled",
				userApi?.Permissions ?? 0);
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
