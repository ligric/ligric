using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ligric.Application.UserApis;
using Ligric.Application.UserApis.CreateUserApi;
using Ligric.Protos;
using Ligric.Server.Grpc.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.Server.Grpc.Services
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
				Result = ResponseExtensions.GetSuccessResponseResult(),
				ApiId = apiId
			};
		}

		[Authorize]
		public override async Task ApisSubscribe(ApiSubscribeRequest request, IServerStreamWriter<ApisChanged> responseStream, ServerCallContext context)
		{
			await _userApiObserver.GetApisAsObservable(request.UserId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					await responseStream.WriteAsync(new ApisChanged
					{
						Action = x.Action.ToProtosAction(),
						Api = new ApiClient
						{
							Id = x.Api.UserApiId ?? throw new ArgumentNullException("ApisSubscribe UserApiId is null"),
							Name = x.Api.Name,
							Permissions = x.Api.Permissions
						}
					});
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
	}
}
