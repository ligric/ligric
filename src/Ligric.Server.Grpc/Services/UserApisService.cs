using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ligric.Application.UserApis.CreateUserApi;
using Ligric.Protos;
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

		public UserApisService(IMediator mediator)
		{
			_mediator = mediator;
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
		public override async Task ApisSubscribe(Empty empty, IServerStreamWriter<ApisChanged> responseStream, ServerCallContext context)
		{
			//await _chatService.GetMessagesAsObservable()
			//		.ToAsyncEnumerable()
			//		.ForEachAwaitAsync(async (x) => await responseStream.WriteAsync(
			//			new LobbyMessageResponse
			//			{
			//				Action = x.Action.ToProtosAction(),
			//				LobbyMessage = new LobbyMessage
			//				{
			//					MessageGuid = x.Message.Guid.ToString(),
			//					Message = x.Message.Content,
			//					Time = x.Message.DateTime.ToTimestamp(),
			//					UserGuid = x.Message.User.UserGuid.ToString(),
			//					Username = x.Message.User.Name
			//				}
			//			}, context.CancellationToken))
			//		.ConfigureAwait(false);
		}
	}
}
