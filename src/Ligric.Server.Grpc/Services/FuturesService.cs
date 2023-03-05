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
		public override async Task OrdersSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<OrdersChanged> responseStream, ServerCallContext context)
		{
			await _futuresObserver.GetOrdersAsObservable(request.UserId, request.UserApiId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					if (x.UserIds.Contains(request.UserId))
					{
						var eventArgs = x.EventArgs;
						FutureOrder? order = null;
						if (eventArgs.Action == Utils.NotifyDictionaryChangedAction.Removed)
						{
							order = new FutureOrder { Id = eventArgs.Key };
						}
						else if (eventArgs.NewValue != null)
						{
							order = eventArgs.NewValue.ToFutureOrder();
						}

						var orderChanged = new OrdersChanged
						{
							Action = x.EventArgs.Action.ToProtosAction(),
							Order = order ?? throw new NullReferenceException("[ForEachAwaitAsync] order is null")
						};

						await responseStream.WriteAsync(orderChanged);
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
		{
			await _futuresObserver.GetOrdersAsObservable(request.UserId, request.UserApiId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					if (x.UserIds.Contains(request.UserId))
					{
						var eventArgs = x.EventArgs;
						FutureOrder? order = null;
						if (eventArgs.Action == Utils.NotifyDictionaryChangedAction.Removed)
						{
							order = new FutureOrder { Id = eventArgs.Key };
						}
						else if (eventArgs.NewValue != null)
						{
							order = eventArgs.NewValue.ToFutureOrder();
						}

						var orderChanged = new OrdersChanged
						{
							Action = x.EventArgs.Action.ToProtosAction(),
							Order = order ?? throw new NullReferenceException("[ForEachAwaitAsync] order is null")
						};

						await responseStream.WriteAsync(orderChanged);
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
	}
}
