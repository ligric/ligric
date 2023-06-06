using Grpc.Core;
using Ligric.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Service.CryptoApisService.Api.Helpers;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;

namespace Ligric.Service.CryptoApisService.Api.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BinanceFuturesTradesService : BinanceFuturesTrades.BinanceFuturesTradesBase
	{
		private readonly BinanceTradeSubscriptions _tradeSubscriptions;

		public BinanceFuturesTradesService(BinanceTradeSubscriptions tradeSubscriptions)
		{
			_tradeSubscriptions = tradeSubscriptions;
		}

		[Authorize]
		public override async Task TradesSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<TradesChanged> responseStream, ServerCallContext context)
		{
			//Guid subscribedId = Guid.Empty;
			try
			{
				await _tradeSubscriptions
					.GetTradesAsObservable()
					.ToAsyncEnumerable()
					.ForEachAwaitAsync(async (x) =>
					{
						var orderChanged = new TradesChanged
						{
							Action = x.EventArgs.Action.ToProtosAction(),
							Trade = new FuturesTrade
							{
								Symbol = x.EventArgs.Key ?? throw new ArgumentException("[ValuesSubscribe] Key is null."),
								Value = x.EventArgs.NewValue.ToString()
							}
						};
						await responseStream.WriteAsync(orderChanged);
					}, context.CancellationToken)
					.ConfigureAwait(false);
			}
			catch (TaskCanceledException)
			{
				//_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				//System.Diagnostics.Debug.WriteLine($"Trades subscribtion {subscribedId} was canceled.");
			}
			catch (Exception)
			{
				//System.Diagnostics.Debug.WriteLine($"Trades subscribtion {subscribedId} thrown an error: \n{ex}.");
				//_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}

		[Authorize]
		public override async Task<AddStreamingApiResponse> AddTradesStreamingApi(AddStreamingApiRequest request, ServerCallContext context)
		{
			_tradeSubscriptions.SetSubscribedStream(request.UserId, request.UserApiId, out var streamSubscriptionId);
			return new AddStreamingApiResponse
			{
				Result = ResponseHelper.GetSuccessResponseResult(),
				StreamSubscriptionId = streamSubscriptionId.ToString()
			};
		}

		[Authorize]
		public override async Task<ResponseResult> RemoveTradesStreamingApi(RemoveStreamingApiRequest request, ServerCallContext context)
		{
			if (Guid.TryParse(request.StreamSubscribedId, out var streamSubscribedId))
			{
				_tradeSubscriptions.UnsubscribeStream(streamSubscribedId);
				return ResponseHelper.GetSuccessResponseResult();
			}
			else
			{
				return ResponseHelper.GetFailedResponseResult();
			}
		}
	}
}
