using System.Text.Json.Serialization;

namespace Ligric.Backend.Infrastructure.Jwt
{
	public class JwtAuthResult
	{
		[JsonPropertyName("accessToken")]
		public string? AccessToken { get; set; }

		[JsonPropertyName("refreshToken")]
		public RefreshToken? RefreshToken { get; set; }
	}
}
