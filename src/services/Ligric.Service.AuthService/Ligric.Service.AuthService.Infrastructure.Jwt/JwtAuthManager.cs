using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ligric.Service.AuthService.Infrastructure.Jwt
{
	public class JwtAuthManager : IJwtAuthManager
	{
		public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
		private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;  // can store in a database or a distributed cache
		private readonly JwtTokenConfig _jwtTokenConfig;
		private readonly byte[] _secret;

		public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
		{
			_jwtTokenConfig = jwtTokenConfig;
			_usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
			_secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
		}

		public DateTime GetTokenExpirationTime(string userUniqueName)
		{
			var expiredAt = _usersRefreshTokens.Values.First(x => x.UserName == userUniqueName).ExpireAt;
			return expiredAt;
		}

		// optional: clean up expired refresh tokens
		public void RemoveExpiredRefreshTokens(DateTime utcNow)
		{
			var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < utcNow).ToList();
			foreach (var expiredToken in expiredTokens)
			{
				_usersRefreshTokens.TryRemove(expiredToken.Key, out _);
			}
		}

		// can be more specific to ip, user agent, device name, etc.
		public void RemoveRefreshTokenByUserName(string userName)
		{
			var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
			foreach (var refreshToken in refreshTokens)
			{
				_usersRefreshTokens.TryRemove(refreshToken.Key, out _);
			}
		}

		public JwtAuthResult GenerateTokens(string? username, IEnumerable<Claim> claims, DateTime utcNow)
		{
			var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
			var accessTokenExpairedAt = utcNow.AddMinutes(_jwtTokenConfig.AccessTokenExpiration);
			var jwtToken = new JwtSecurityToken(
				_jwtTokenConfig.Issuer,
				shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
				claims,
				expires: accessTokenExpairedAt,
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
			var accessTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			var refreshToken = new RefreshToken
			{
				UserName = username,
				TokenString = GenerateRefreshTokenString(),
				ExpireAt = utcNow.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
			};

			var accessToken = new AccessToken
			{
				UserName = username,
				TokenString = accessTokenString,
				ExpireAt = accessTokenExpairedAt
			};
			_usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (_, _) => refreshToken);

			return new JwtAuthResult
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime utcNow)
		{
			var (principal, jwtToken) = DecodeJwtToken(accessToken);
			if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
			{
				throw new SecurityTokenException("Invalid token");
			}

			var userName = principal.Identity?.Name;
			if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
			{
				throw new SecurityTokenException("Invalid token");
			}
			if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < utcNow)
			{
				throw new SecurityTokenException("Invalid token");
			}

			return GenerateTokens(userName, principal.Claims.ToArray(), utcNow); // need to recover the original claims
		}

		public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				throw new SecurityTokenException("Invalid token");
			}
			var principal = new JwtSecurityTokenHandler()
				.ValidateToken(token,
					new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = _jwtTokenConfig.Issuer,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(_secret),
						ValidAudience = _jwtTokenConfig.Audience,
						ValidateAudience = true,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(1)
					},
					out var validatedToken);
			return (principal, (JwtSecurityToken)validatedToken);
		}

		private static string GenerateRefreshTokenString()
		{
			var randomNumber = new byte[32];
			using var randomNumberGenerator = RandomNumberGenerator.Create();
			randomNumberGenerator.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}
}
