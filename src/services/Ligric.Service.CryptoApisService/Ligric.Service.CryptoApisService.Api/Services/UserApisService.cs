using Grpc.Core;
using Ligric.Protobuf;
using Ligric.Service.CryptoApisService.Api.Helpers;
using Ligric.Service.CryptoApisService.Application.TemporaryObservers;
using Ligric.Service.CryptoApisService.UseCase.Handlers.CreateUserApi;
using Ligric.Service.CryptoApisService.UseCase.Handlers.ShareUserApi;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.Service.CryptoApisService.Api.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UserApisService : UserApis.UserApisBase
	{
		private const int OWNER_API_PERMISSION = 31;

		private readonly IMediator _mediator;
		private readonly IUserApiObserver _userApiObserver;

		public UserApisService(
			IMediator mediator,
			IUserApiObserver userApiObserver)
		{
			_mediator = mediator;
			_userApiObserver = userApiObserver;
		}

		[Authorize]
		public override async Task<SaveApiResponse> Save(SaveApiRequest saveApi, ServerCallContext context)
		{
			var createUserApiCommand = new CreateUserApiCommand(
				saveApi.Name,
				saveApi.OwnerId,
				saveApi.PrivateKey,
				saveApi.PublicKey,
				OWNER_API_PERMISSION);

			var apiId = await _mediator.Send(createUserApiCommand);

			return new SaveApiResponse
			{
				Result = ResponseHelper.GetSuccessResponseResult(),
				ApiId = apiId
			};
		}

		[Authorize]
		public override async Task<ResponseResult> Share(ShareApiRequest shareRequest, ServerCallContext context)
		{
			var shareUserApiCommand = new ShareUserApiCommand(shareRequest.UserApiId, shareRequest.Permissions, new List<long>());
			var isSuccess = await _mediator.Send(shareUserApiCommand);

			return isSuccess ? ResponseHelper.GetSuccessResponseResult() : ResponseHelper.GetFailedResponseResult();
		}

		[Authorize]
		public override async Task ApisSubscribe(ApiSubscribeRequest request, IServerStreamWriter<ApisChanged> responseStream, ServerCallContext context)
		{
			await _userApiObserver.GetApisAsObservable(request.UserId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					if (x.UserId == request.UserId)
					{
						await responseStream.WriteAsync(new ApisChanged
						{
							Action = x.Action.ToProtosAction(),
							Api = new ApiClient
							{
								Id = x.Api.UserApiId,
								Name = x.Api.Name,
								Permissions = x.Api.Permissions
							}
						});
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
	}
}
