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
	public class UserApisService : UserApies.UserApiesBase
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
				int.Parse(saveApi.OwnerId),
				saveApi.PrivateKey,
				saveApi.PublicKey,
				OWNER_API_PERMISSION);

			var apiId = await _mediator.Send(createUserApiCommand);

			return new SaveApiResponse
			{
				Result = ResponseExtensions.GetSuccessResponseResult(),
			    ApiId = apiId.ToString()
			};
		}

		[Authorize]
		public override async Task ApiesSubscribe(Empty empty, IServerStreamWriter<ApiesChanged> responseStream, ServerCallContext context)
		{
			throw new NotImplementedException();
		}
	}
}
