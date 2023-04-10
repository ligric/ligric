﻿using System.Text.Json.Serialization;

namespace Ligric.Service.AuthService.Infrastructure.Jwt
{
	public class JwtAuthResult
	{
		[JsonPropertyName("accessToken")]
		public string? AccessToken { get; set; }

		[JsonPropertyName("refreshToken")]
		public RefreshToken? RefreshToken { get; set; }
	}
}
