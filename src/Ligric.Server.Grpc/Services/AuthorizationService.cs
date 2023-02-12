using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Server.Domain.SharedKernel;
using Ligric.Protos;
using MediatR;
using Google.Protobuf.WellKnownTypes;
using Ligric.Application.Users.CheckUserExists;
using Ligric.Infrastructure.Jwt;
using System.Security.Claims;
using Ligric.Domain.Types.User;

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

		//var registerCommand = new LoginUserCommand(request.Login, request.Password);
		//var user = await _mediator.Send(registerCommand);

		// check user password
		// _usersService.ValidateCredentials(login, password);

		// get role
		// var role = _userService.GetRole(login);

		// for example

		var claims = new[]
		{
			new Claim(ClaimTypes.Name, request.Login)
		};

		var token = _jwtAuthManager.GenerateTokens(request.Login, claims, DateTime.Now);
		//_logger.LogInformation($"User [{request.UserName}] logged in the system.");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
		var result = new SignInResponse
		{
			Role = string.Empty,
			JwtToken = new JwtToken
			{
				AccessToken = token.AccessToken,
				RefreshToken = token.RefreshToken.TokenString,
				ExpirationAt = Timestamp.FromDateTime(token.RefreshToken.ExpireAt.SetKind(DateTimeKind.Utc))
			},
			Result = ResponseExtensions.GetSuccessResponseResult()
		};
#pragma warning restore CS8602 // Dereference of a possibly null reference.
		return result;
	}

	//	[AllowAnonymous]
	//	public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
	//	{
	//		throw new NotImplementedException();

	//		//var registerCommand = new RegisterUserCommand(request.Login, request.Password);
	//		//var customer = await _mediator.Send(registerCommand);
	//		//var userRoles = new List<string> { "User" };
	//		//var token = GetJwtToken(customer, userRoles);

	//		//return new SignUpResponse()
	//		//{
	//		//    Result = ResponseExtensions.GetSuccessResponseResult(),
	//		//    UserGuid = customer.Id.ToString(),
	//		//    JwtToken = token.JwtToken,
	//		//    TokenExpiration = Timestamp.FromDateTime(token.Expiration)
	//		//};
	//	}

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
