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
	public class BinanceFuturesPositionsService : BinanceFuturesPositions.BinanceFuturesPositionsBase
	{
		private readonly BinancePositionSubscriptions _positionSubscriptions;

		public BinanceFuturesPositionsService(BinancePositionSubscriptions positionSubscriptions)
		{
			_positionSubscriptions = positionSubscriptions;
		}

		[Authorize]
		public override async Task PositionsSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<PositionsChanged> responseStream, ServerCallContext context)
		{
			//Guid subscribedId = Guid.Empty;
			try
			{
				await _positionSubscriptions
					.GetPositionsAsObservable()
					.ToAsyncEnumerable()
					.ForEachAwaitAsync(async (x) =>
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
							ExchangeId = x.ExchangeId.ToString(),
							Action = x.EventArgs.Action.ToProtosAction(),
							Position = position ?? (x.EventArgs.Action == Utils.NotifyDictionaryChangedAction.Cleared ? null : throw new NullReferenceException("[ForEachAwaitAsync] order is null"))
						};

						await responseStream.WriteAsync(positionsChanged);
					}, context.CancellationToken)
					.ConfigureAwait(false);
			}
			catch (TaskCanceledException)
			{
				//_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				//System.Diagnostics.Debug.WriteLine($"Positions subscribtion {subscribedId} was canceled.");
			}
			catch (Exception ex)
			{
				//System.Diagnostics.Debug.WriteLine($"Positions subscribtion {subscribedId} thrown an error: \n{ex}.");
				//_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}

		[Authorize]
		public override async Task<AddStreamingApiResponse> AddPositionsStreamingApi(AddStreamingApiRequest request, ServerCallContext context)
		{
			_positionSubscriptions.SetSubscribedStream(request.UserId, request.UserApiId, out var streamSubscriptionId);
			return new AddStreamingApiResponse
			{
				Result = ResponseHelper.GetSuccessResponseResult(),
				StreamSubscriptionId = streamSubscriptionId.ToString()
			};
		}
	}
}
