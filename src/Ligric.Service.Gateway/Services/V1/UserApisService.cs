using Grpc.Core;
using Ligric.Rpc.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.Service.Gateway.Services.V1
{
	public class UserApisService : UserApis.UserApisBase
	{
		private readonly UserApis.UserApisClient _client;

		public UserApisService(UserApis.UserApisClient client)
		{
			_client = client;
		}

		public override async Task<SaveApiResponse> Save(SaveApiRequest saveApi, ServerCallContext context)
		{
			return await _client.SaveAsync(
				request: saveApi,
				headers: context.RequestHeaders,
				cancellationToken: context.CancellationToken);
		}

		public override async Task<ResponseResult> Share(ShareApiRequest shareRequest, ServerCallContext context)
		{
			return await _client.ShareAsync(
				request: shareRequest,
				headers: context.RequestHeaders,
				cancellationToken: context.CancellationToken);
		}

		public override async Task ApisSubscribe(ApiSubscribeRequest request, IServerStreamWriter<ApisChanged> responseStream, ServerCallContext context)
		{
			//await _client.ApisSubscribe(
			//	request: request,
			//	headers: context.RequestHeaders,
			//	cancellationToken: context.CancellationToken);
		}
	}
}
