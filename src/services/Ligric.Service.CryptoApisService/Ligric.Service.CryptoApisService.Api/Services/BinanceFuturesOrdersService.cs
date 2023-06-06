using Grpc.Core;
using Ligric.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Service.CryptoApisService.Api.Extensions;
using Ligric.Service.CryptoApisService.Api.Helpers;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance;

namespace Ligric.Service.CryptoApisService.Api.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BinanceFuturesOrdersService : BinanceFuturesOrders.BinanceFuturesOrdersBase
	{
		private readonly BinanceOrderSubscriptions _ordersSubscriptions;

		public BinanceFuturesOrdersService(BinanceOrderSubscriptions ordersSubscriptions)
		{
			_ordersSubscriptions = ordersSubscriptions;
		}

		[Authorize]
		public override async Task OrdersSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<OrdersChanged> responseStream, ServerCallContext context)
		{
			//Guid subscribedStreamId = Guid.Empty;
			try
			{
				await _ordersSubscriptions
					.GetOrdersAsObservable()
					.ToAsyncEnumerable()
					.ForEachAwaitAsync(async (x) =>
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
							ExchangeId = x.ExchangeId.ToString(),
							Action = x.EventArgs.Action.ToProtosAction(),
							Order = order ?? (x.EventArgs.Action == Utils.NotifyDictionaryChangedAction.Cleared ? null : throw new NullReferenceException("[ForEachAwaitAsync] order is null"))
						};

						await responseStream.WriteAsync(orderChanged);
					}, context.CancellationToken)
				.ConfigureAwait(false);
			}
			catch (TaskCanceledException)
			{
				//_ordersSubscriptions.UnsubscribeStream();
				//System.Diagnostics.Debug.WriteLine($"Orders subscribtion {subscribedStreamId} was canceled.");
			}
			catch (Exception)
			{
				//System.Diagnostics.Debug.WriteLine($"Orders subscribtion {subscribedStreamId} thrown an error: \n{ex}.");
				//_ordersSubscriptions.UnsubscribeStream();
				throw;
			}
		}

		[Authorize]
		public override async Task<AddStreamingApiResponse> AddOrdersStreamingApi(AddStreamingApiRequest request, ServerCallContext context)
		{
			_ordersSubscriptions.SetSubscribedStream(request.UserId, request.UserApiId, out var streamSubscriptionId);
			return new AddStreamingApiResponse
			{
				Result = ResponseHelper.GetSuccessResponseResult(),
				StreamSubscriptionId = streamSubscriptionId.ToString()
			};
		}
	}
}
