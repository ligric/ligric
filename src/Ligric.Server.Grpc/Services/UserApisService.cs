using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ligric.Protos;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.Server.Grpc.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UserApisService : UserApies.UserApiesBase
	{
		private readonly IMediator _mediator;

		public UserApisService(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Authorize]
		public override async Task<SaveApiResponse> Save(SaveApiRequest saveApi, ServerCallContext context)
		{
			throw new NotImplementedException();
		}

		[Authorize]
		public override async Task ApiesSubscribe(Empty empty, IServerStreamWriter<ApiesChanged> responseStream, ServerCallContext context)
		{
			throw new NotImplementedException();
		}
	}
}
