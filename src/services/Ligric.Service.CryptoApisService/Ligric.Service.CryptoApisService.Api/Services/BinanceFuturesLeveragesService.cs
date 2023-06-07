using Grpc.Core;
using Ligric.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Service.CryptoApisService.Api.Helpers;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance;
using Google.Protobuf.WellKnownTypes;

namespace Ligric.Service.CryptoApisService.Api.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BinanceFuturesLeveragesService : BinanceFuturesLeverages.BinanceFuturesLeveragesBase
	{
		private readonly BinanceLeverageSubscriptions _leverageSubscriptions;

		public BinanceFuturesLeveragesService(BinanceLeverageSubscriptions leverageSubscriptions)
		{
			_leverageSubscriptions = leverageSubscriptions;
		}

		[Authorize]
		public override async Task LeveragesSubscribe(Empty request, IServerStreamWriter<LeveragesChanged> responseStream, ServerCallContext context)
		{
			Guid subscribedId = Guid.Empty;
			try
			{
				await _leverageSubscriptions
					.GetLeveragesAsObservable()
					.ToAsyncEnumerable()
					.ForEachAwaitAsync(async (x) =>
					{
						var orderChanged = new LeveragesChanged
						{
							ExchangeId = x.ExchangeId.ToString(),
							Action = x.EventArgs.Action.ToProtosAction(),
							Leverage = new FuturesLeverage
							{
								Symbol = x.EventArgs.Key,
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
				//System.Diagnostics.Debug.WriteLine($"Leverages subscribtion {subscribedId} was canceled.");
			}
			catch (Exception)
			{
				//System.Diagnostics.Debug.WriteLine($"Leverages subscribtion {subscribedId} thrown an error: \n{ex}.");
				//_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}

		[Authorize]
		public override async Task<AddStreamingApiResponse> AddLeveragesStreamingApi(AddStreamingApiRequest request, ServerCallContext context)
		{
			_leverageSubscriptions.SetSubscribedStream(request.UserId, request.UserApiId, out var streamSubscriptionId);
			return new AddStreamingApiResponse
			{
				Result = ResponseHelper.GetSuccessResponseResult(),
				StreamSubscriptionId = streamSubscriptionId.ToString()
			};
		}

		[Authorize]
		public override async Task<ResponseResult> RemoveLeveragesStreamingApi(RemoveStreamingApiRequest request, ServerCallContext context)
		{
			if (Guid.TryParse(request.StreamSubscribedId, out var streamSubscribedId))
			{
				_leverageSubscriptions.UnsubscribeStream(streamSubscribedId);
				return ResponseHelper.GetSuccessResponseResult();
			}
			else
			{
				return ResponseHelper.GetFailedResponseResult();
			}
		}
	}
}
