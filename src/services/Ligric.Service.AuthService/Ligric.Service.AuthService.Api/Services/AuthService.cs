using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MediatR;
using Google.Protobuf.WellKnownTypes;
using System.Security.Claims;
using Ligric.Service.AuthService.Infrastructure.Jwt;
using Ligric.Service.AuthService.Domain.SharedKernel;
using Ligric.Core.Types.User;
using Ligric.Service.AuthService.Api.Helpers;
using Ligric.Service.AuthService.UseCase.Handlers.CheckUserExists;
using Ligric.Service.AuthService.UseCase.Handlers.LoginCustomer;
using Ligric.Service.AuthService.UseCase.Handlers.RegisterUser;
using Ligric.Protobuf;

namespace Ligric.Service.AuthService.Api.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthService : Auth.AuthBase
{
	private readonly IMediator _mediator;
	private readonly IJwtAuthManager _jwtAuthManager;

	public AuthService(
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
			Result = ResponseHelper.GetSuccessResponseResult(),
			IsUnique = isUnique
		};
	}

	[AllowAnonymous]
	public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
	{
		if (request?.Login == null || request?.Password == null)
		{
			return new SignInResponse { Result = ResponseHelper.GetFailedResponseResult() };
		}

		var registerCommand = new LoginUserCommand(request.Login, request.Password);
		var user = await _mediator.Send(registerCommand);

		if (user != null && user.UserName != null)
		{
			var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
			var token = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.UtcNow);

			return new SignInResponse
			{
				Id = (long)user.Id,
				JwtToken = new JwtToken
				{
					AccessToken = token.AccessToken?.TokenString ?? throw new ArgumentNullException("Response AccessToken is null"),
					RefreshToken = token.RefreshToken?.TokenString ?? throw new ArgumentNullException("Response RefreshToken is null"),
					ExpirationAt = Timestamp.FromDateTime(token.AccessToken.ExpireAt.SetKind(DateTimeKind.Utc))
				},
				Result = ResponseHelper.GetSuccessResponseResult()
			};
		}

		return new SignInResponse
		{
			Result = ResponseHelper.GetFailedResponseResult()
		};
	}

	[AllowAnonymous]
	public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
	{
		var registerCommand = new RegisterUserCommand(request.Login, request.Password);
		var user = await _mediator.Send(registerCommand);

		if (user != null && user.UserName != null)
		{
			var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
			var token = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.UtcNow);

			return new SignUpResponse
			{
				Id = (long)user.Id,
				JwtToken = new JwtToken
				{
					AccessToken = token.AccessToken?.TokenString ?? throw new ArgumentNullException("Response AccessToken is null"),
					RefreshToken = token.RefreshToken?.TokenString ?? throw new ArgumentNullException("Response RefreshToken is null"),
					ExpirationAt = Timestamp.FromDateTime(token.AccessToken.ExpireAt.SetKind(DateTimeKind.Utc))
				},
				Result = ResponseHelper.GetSuccessResponseResult()
			};
		}

		return new SignUpResponse
		{
			Result = ResponseHelper.GetFailedResponseResult()
		};
	}

	[Authorize]
	public override async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
	{
		var headers = context.RequestHeaders;
		var accessToken = headers.First(x => x.Key == "authorization").Value.Replace("Bearer ", string.Empty);
		var token = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.UtcNow);
		return new RefreshTokenResponse
		{
			JwtToken = new JwtToken
			{
				AccessToken = token.AccessToken?.TokenString ?? throw new ArgumentNullException("Response AccessToken is null"),
				RefreshToken = token.RefreshToken?.TokenString ?? throw new ArgumentNullException("Response RefreshToken is null"),
				ExpirationAt = Timestamp.FromDateTime(token.AccessToken.ExpireAt.SetKind(DateTimeKind.Utc))
			},
			Result = ResponseHelper.GetSuccessResponseResult()
		};
	}
}
