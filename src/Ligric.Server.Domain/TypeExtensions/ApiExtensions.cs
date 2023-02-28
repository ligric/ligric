using Ligric.Domain.Types.Api;
using Ligric.Server.Domain.Entities.Apies;

namespace Ligric.Server.Domain.TypeExtensions
{
	public static class ApiExtensions
	{
		public static ApiDto ToApiDto(this ApiEntity api)
		{
			return new ApiDto(
				api.Id,
				api.PublicKey ?? throw new System.ArgumentException($"Api [{api.Id}] public key is null"),
				api.PrivateKey ?? throw new System.ArgumentException($"Api [{api.Id}] private key is null"));
		}

	}
}
