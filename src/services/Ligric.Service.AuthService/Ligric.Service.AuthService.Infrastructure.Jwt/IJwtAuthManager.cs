using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ligric.Service.AuthService.Infrastructure.Jwt
{
	public interface IJwtAuthManager
	{
		IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
		JwtAuthResult GenerateTokens(string username, IEnumerable<Claim> claims, DateTime utcNow);
		JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime utcNow);
		DateTime GetTokenExpirationTime(string userUniqueName);
		void RemoveExpiredRefreshTokens(DateTime utcNow);
		void RemoveRefreshTokenByUserName(string userName);
		(ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
	}
}
