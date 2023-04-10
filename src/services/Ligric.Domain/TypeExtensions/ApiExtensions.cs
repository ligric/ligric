using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Domain.Entities.Apies;

namespace Ligric.Domain.TypeExtensions
{
	public static class ApiExtensions
	{
		public static ApiDto ToApiDto(this ApiEntity api)
		{
			return new ApiDto(
				api.Id ?? throw new System.ArgumentException($"Api [{api.Id}] id is null"),
				api.PublicKey ?? throw new System.ArgumentException($"Api [{api.Id}] public key is null"),
				api.PrivateKey ?? throw new System.ArgumentException($"Api [{api.Id}] private key is null"));
		}

	}
}
