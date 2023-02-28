using Grpc.Core;
using Ligric.Protos;
using Ligric.Server.Grpc.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.Server.Grpc.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class FutureOrdersService : FutureOrders.FutureOrdersBase
	{
		private readonly IMediator _mediator;

		public FutureOrdersService(
			IMediator mediator)
		{
			_mediator = mediator;
		}

		[Authorize]
		public override async Task<ResponseResult> AddObserverUserApi(AddObserverUserApiRequest request, ServerCallContext context)
		{
			throw new NotImplementedException();
		}

		[Authorize]
		public override async Task OrdersSubscribe(OrdersSubscribeRequest request, IServerStreamWriter<OrdersChanged> responseStream, ServerCallContext context)
		{
			throw new NotImplementedException();
			//await _userApiObserver.GetApisAsObservable(request.UserId)
			//	.ToAsyncEnumerable()
			//	.ForEachAwaitAsync(async (x) =>
			//	{
			//		if (x.UserId == request.UserId)
			//		{
			//			await responseStream.WriteAsync(new ApisChanged
			//			{
			//				Action = x.Action.ToProtosAction(),
			//				Api = new ApiClient
			//				{
			//					Id = x.Api.UserApiId ?? throw new ArgumentNullException("ApisSubscribe UserApiId is null"),
			//					Name = x.Api.Name,
			//					Permissions = x.Api.Permissions
			//				}
			//			});
			//		}
			//	}, context.CancellationToken)
			//	.ConfigureAwait(false);
		}
	}
}
