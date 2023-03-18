using Grpc.Core;
using Ligric.Rpc.Contracts;

namespace Ligric.Service.Gateway.Services.V1
{
	public class AuthService : Auth.AuthBase
	{
		private readonly Auth.AuthClient _client;

		public AuthService(Auth.AuthClient authClient)
		{
			_client = authClient;
		}

		public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
		{
			return await _client.SignInAsync(
				request: request,
				cancellationToken: context.CancellationToken);
		}

		public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
		{
			return await _client.SignUpAsync(
				request: request,
				cancellationToken: context.CancellationToken);
		}
	}

}
