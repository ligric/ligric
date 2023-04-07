using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ligric.Infrastructure.Jwt
{
	public interface IJwtAuthManager
	{
		IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
		JwtAuthResult GenerateTokens(string username, IEnumerable<Claim> claims, DateTime now);
		JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
		DateTime GetTokenExpirationTime(string userUniqueName);
		void RemoveExpiredRefreshTokens(DateTime now);
		void RemoveRefreshTokenByUserName(string userName);
		(ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
	}
}
