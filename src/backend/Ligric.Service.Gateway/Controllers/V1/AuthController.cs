using Microsoft.AspNetCore.Mvc;
using static Ligric.Rpc.Contracts.Auth;

namespace Ligric.Service.Gateway.Controllers.V1
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : BaseController
	{
		private readonly ILogger<AuthController> _logger;
		private readonly AuthClient _authClient;

		public AuthController(
			ILogger<AuthController> logger,
			AuthClient authClient)
		{
			_logger = logger;
			_authClient = authClient;
		}

		//[HttpPost("verify-code")]
		//public async Task<AuthUserInfoResponseDto> VerifyCode(VerifyCodeRequestDto dto)
		//{
		//	return await OnExecution(async () =>
		//	{
		//		var response = await _userClient.VerifyCodeAsync(new VerifyCodeRequest()
		//		{
		//			Token = dto.Code,
		//			PhoneNumber = dto.PhoneNumber,
		//		});


		//		return new AuthUserInfoResponseDto
		//		{
		//			PhoneNumber = response.PhoneNumber,
		//			Id = response.Id,
		//			Token = response.Token
		//		};
		//	});
		//}
	}
}
