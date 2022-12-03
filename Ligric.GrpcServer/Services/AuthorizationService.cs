using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ligric.Protos;
using Ligric.GrpcServer.Data;
using MediatR;
using Ligric.Application.Users;
using Ligric.Application.Users.CheckUserExists;
using Ligric.Application.Users.RegisterUser;
using Google.Protobuf.WellKnownTypes;

namespace Ligric.GrpcServer.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthorizationService : Authorization.AuthorizationBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    //private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(IMediator mediator, IConfiguration configuration/*, ILogger<AuthorizationService> logger*/)
    {
        _mediator = mediator;
        _configuration = configuration;
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
    public override Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
    {
        var registerCommand = new RegisterUserCommand(request.Login, request.Password);
        var customer = await _mediator.Send(registerCommand);
        var userRoles = new List<string> { "User" };
        var token = GetJwtToken(customer, userRoles);

        return new SignUpResponse()
        {
            Result = ResponseExtensions.GetSuccessResponseResult(),
            UserGuid = customer.Id.ToString(),
            JwtToken = token.JwtToken,
            TokenExpiration = Timestamp.FromDateTime(token.Expiration)
        };
    }

    #region #HARD_CODE. Reason: deadline.
    private Token GetJwtToken(UserDto applicationUserDto, IEnumerable<string> roles)
    {
        var applicationUserClaims = GetApplicationUserClaims(applicationUserDto);
        var applicationUserRolesClaims = GetRolesAsClaims(roles);
        var jwtAuthRequiredClaims = GetJwtAuthRequiredClaims(_configuration.GetValue<string>("JwtIssuer"), _configuration.GetValue<string>("JwtAudience"));
        var claims = jwtAuthRequiredClaims.Union(applicationUserRolesClaims).Union(applicationUserClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtKey")));
        var signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtHeader = new JwtHeader(signingCredential);
        var jwtPayload = new JwtPayload(claims);
        var token = new JwtSecurityToken(jwtHeader, jwtPayload);
        return new Token(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }

    private static IEnumerable<Claim> GetApplicationUserClaims(UserDto userDto)
    {
        return new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Login),
            new Claim("Guid", userDto.Id.ToString())
        };
    }

    private static IEnumerable<Claim> GetJwtAuthRequiredClaims(string issuer, string audience)
    {
        return new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(8)).ToUnixTimeSeconds().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, issuer),
            new Claim(JwtRegisteredClaimNames.Aud, audience)
        };
    }

    private static IEnumerable<Claim> GetRolesAsClaims(IEnumerable<string> roles)
    {
        const string roleType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        return roles.Select(x => new Claim(roleType, x));
    }
    #endregion
}
