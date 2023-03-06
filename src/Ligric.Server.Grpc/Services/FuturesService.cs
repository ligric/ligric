using CryptoExchange.Net.CommonObjects;
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

		[Authorize]
		public override async Task ValuesSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<ValuesChanged> responseStream, ServerCallContext context)
		{
			await _futuresObserver.GetValuesAsObservable(request.UserId, request.UserApiId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					if (x.UserIds.Contains(request.UserId))
					{
						var orderChanged = new ValuesChanged
						{
							Action = x.EventArgs.Action.ToProtosAction(),
							Symbol = x.EventArgs.Key ?? throw new ArgumentException("[ValuesSubscribe] Key is null."),
							Value = x.EventArgs.NewValue.ToString()
						};

						await responseStream.WriteAsync(orderChanged);
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
	}
}
