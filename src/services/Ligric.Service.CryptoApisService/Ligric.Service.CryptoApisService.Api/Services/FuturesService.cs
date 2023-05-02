﻿using Grpc.Core;
using Ligric.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Service.CryptoApisService.Api.Extensions;
using Ligric.Service.CryptoApisService.Api.Helpers;
using Ligric.Service.CryptoApisService.Application;

namespace Ligric.Service.CryptoApisService.Api.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class FuturesService : Futures.FuturesBase
	{
		private ITemporaryUserFuturesObserver _futuresObserver;

		public FuturesService(
			ITemporaryUserFuturesObserver futuresObserver)
		{
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
						FuturesOrder? order = null;
						if (eventArgs.Action == Utils.NotifyDictionaryChangedAction.Removed)
						{
							order = new FuturesOrder { Id = eventArgs.Key };
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
							Value = new FuturesValue
							{
							   Symbol = x.EventArgs.Key ?? throw new ArgumentException("[ValuesSubscribe] Key is null."),
							   Value = x.EventArgs.NewValue.ToString()
							}
						};

						await responseStream.WriteAsync(orderChanged);
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}

		[Authorize]
		public override async Task PositionsSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<PositionsChanged> responseStream, ServerCallContext context)
		{
			await _futuresObserver.GetPositionsAsObservable(request.UserId, request.UserApiId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					if (x.UserIds.Contains(request.UserId))
					{
						var eventArgs = x.EventArgs;
						FuturesPosition? position = null;
						if (eventArgs.Action == Utils.NotifyDictionaryChangedAction.Removed)
						{
							position = new FuturesPosition { Id = eventArgs.Key };
						}
						else if (eventArgs.NewValue != null)
						{
							position = eventArgs.NewValue.ToFuturesPosition();
						}

						var positionsChanged = new PositionsChanged
						{
							Action = x.EventArgs.Action.ToProtosAction(),
							Position = position ?? throw new NullReferenceException("[ForEachAwaitAsync] order is null")
						};

						await responseStream.WriteAsync(positionsChanged);
					}
				}, context.CancellationToken)
				.ConfigureAwait(false);
		}
	}
}
