using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;

namespace Ligric.Service.CryptoApisService.Domain.Extensions
{
	public static class UserApiMapExtensions
	{
		public static ApiClientResponseDto ToApiClientResponseDto(this UserApiEntity userApi)
		{
			return new ApiClientResponseDto(
				userApi.Id ?? throw new ArgumentNullException("userApi is null"),
				userApi?.Name ?? "Not titled",
				userApi?.Permissions ?? 0);
		}

		public static UserApiEntity ToUserApiEntity(this ApiClientDto userApi)
		{
			return new UserApiEntity
			{
				Id = userApi.UserApiId,
				Name = userApi.Name,
				Permissions = userApi.Permissions
			};
		}

		public static ApiDto ToApiDto(this ApiEntity entity)
		{
			return new ApiDto(entity.Id ?? throw new ArgumentNullException("Api id is null"),
				entity.PublicKey ?? throw new ArgumentNullException("Api public key is null"),
				entity.PrivateKey ?? throw new ArgumentNullException("Api private key is null"));
		}
	}
}
