using Grpc.Core;
using Ligric.Application.Orders;
using Ligric.Domain.Types.Future;
using Ligric.Protos;
using Ligric.Server.Grpc.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.Server.Grpc.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class FuturesService : Futures.FuturesBase
	{
		private readonly IMediator _mediator;
		private ITemporaryUserFuturesObserver _futuresObserver;

		public FuturesService(
			IMediator mediator,
			ITemporaryUserFuturesObserver futuresObserver)
		{
			_mediator = mediator;
			_futuresObserver = futuresObserver;
		}

		[Authorize]
		public override async Task OrdersSubscribe(OrdersSubscribeRequest request, IServerStreamWriter<OrdersChanged> responseStream, ServerCallContext context)
		{
			await _futuresObserver.GetOrdersAsObservable(request.UserId, request.UserApiId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					if (x.UserIds.Contains(request.UserId))
					{
						await responseStream.WriteAsync(new OrdersChanged
						{
							Action = x.EventArgs.Action.ToProtosAction(),
							Order = x.EventArgs.NewValue?.ToFutureOrder()
						});
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
	}
}
