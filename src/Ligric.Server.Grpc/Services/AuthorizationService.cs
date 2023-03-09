﻿using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Server.Domain.SharedKernel;
using Ligric.Protos;
using MediatR;
using Google.Protobuf.WellKnownTypes;
using Ligric.Server.Application.Users.CheckUserExists;
using Ligric.Server.Infrastructure.Jwt;
using System.Security.Claims;
using Ligric.Domain.Types.User;
using Ligric.Server.Application.Users.LoginCustomer;
using Ligric.Server.Application.Users.RegisterUser;

namespace Ligric.Server.Grpc.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthorizationService : Authorization.AuthorizationBase
{
	private readonly IMediator _mediator;
	private readonly IJwtAuthManager _jwtAuthManager;

	public AuthorizationService(
		IMediator mediator,
		IJwtAuthManager jwtAuthManager)
	{
		_mediator = mediator;
		_jwtAuthManager = jwtAuthManager;
	}

	[AllowAnonymous]
	public override async Task<CheckExistsResponse> IsLoginUnique(CheckExistsRequest request, ServerCallContext context)
	{
		var loginIsUniqueQuery = new UserNameIsUniqueQuery(request.Value);
		bool isUnique = await _mediator.Send(loginIsUniqueQuery);
		return new CheckExistsResponse
		{
			Result = ResponseExtensions.GetSuccessResponseResult(),
			IsUnique = isUnique
		};
	}

	[AllowAnonymous]
	public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
	{
		if (request?.Login == null || request?.Password == null)
		{
			return new SignInResponse { Result = ResponseExtensions.GetFailedResponseResult() };
		}

		var registerCommand = new LoginUserCommand(request.Login, request.Password);
		var user = await _mediator.Send(registerCommand);

		if (user != null && user.UserName != null && user.Id != null)
		{
			var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
			var token = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.Now);

			return new SignInResponse
			{
				Id = (long)user.Id,
				JwtToken = new JwtToken
				{
					AccessToken = token.AccessToken,
					RefreshToken = token.RefreshToken?.TokenString ?? throw new ArgumentNullException("Response RefreshToken is null"),
					ExpirationAt = Timestamp.FromDateTime(token.RefreshToken.ExpireAt.SetKind(DateTimeKind.Utc))
				},
				Result = ResponseExtensions.GetSuccessResponseResult()
			};
		}

		return new SignInResponse
		{
			Result = ResponseExtensions.GetFailedResponseResult()
		};
	}

	[AllowAnonymous]
	public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
	{
		var registerCommand = new RegisterUserCommand(request.Login, request.Password);
		var user = await _mediator.Send(registerCommand);

		if (user != null && user.UserName != null && user.Id != null)
		{
			var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
			var token = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.Now);

			return new SignUpResponse
			{
				Id = (long)user.Id,
				JwtToken = new JwtToken
				{
					AccessToken = token.AccessToken,
					RefreshToken = token.RefreshToken?.TokenString ?? throw new ArgumentNullException("Response RefreshToken is null"),
					ExpirationAt = Timestamp.FromDateTime(token.RefreshToken.ExpireAt.SetKind(DateTimeKind.Utc))
				},
				Result = ResponseExtensions.GetSuccessResponseResult()
			};
		}

		return new SignUpResponse
		{
			Result = ResponseExtensions.GetFailedResponseResult()
		};
	}

	[Authorize]
	public override async Task<RefreshTokenExpirationTime> GetTokenExpirationTime(Empty empty, ServerCallContext context)
	{
		var token = GetTokenFromMetadata(context.RequestHeaders);

		var claims = _jwtAuthManager.DecodeJwtToken(token).Item1.Claims;

		var uniqueName = claims.First(x => x.Type == ClaimTypes.Name).Value;

#pragma warning disable CS8604 // Possible null reference argument.
		var expirationAt = _jwtAuthManager.GetTokenExpirationTime(uniqueName);
#pragma warning restore CS8604 // Possible null reference argument.

		var result = new RefreshTokenExpirationTime
		{
			ExpirationAt = Timestamp.FromDateTime(expirationAt),
			Result = ResponseExtensions.GetSuccessResponseResult()
		};
		return result;
	}

	private UserDto? GetUserFromMetadata(Metadata metadata)
	{
		if (metadata == null)
#pragma warning disable CS8604 // Possible null reference argument.
			throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"), metadata);
#pragma warning restore CS8604 // Possible null reference argument.

		var token = metadata.FirstOrDefault(x => x.Key == "authorization");

		if (token?.Value == null)
			throw new RpcException(new Status(StatusCode.NotFound, "Token is null"), metadata);

		//return _usersService.GetUserFromToken(token.Value);
		return null;
	}

	private string GetTokenFromMetadata(Metadata metadata)
	{
		if (metadata == null)
#pragma warning disable CS8604 // Possible null reference argument.
			throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"), metadata);
#pragma warning restore CS8604 // Possible null reference argument.

		var token = metadata.FirstOrDefault(x => x.Key == "authorization");

		if (token?.Value == null)
			throw new RpcException(new Status(StatusCode.NotFound, "Token is null"), metadata);

		return token.Value;
	}
}
