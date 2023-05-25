using Grpc.Core;
using Ligric.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Service.CryptoApisService.Api.Extensions;
using Ligric.Service.CryptoApisService.Api.Helpers;
using Ligric.Service.CryptoApisService.Application.Observers.Futures;

namespace Ligric.Service.CryptoApisService.Api.Services
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class FuturesService : Futures.FuturesBase
	{
		private IFuturesApiSubscribtionsObserverManager _futuresObserver;

		public FuturesService(
			IFuturesApiSubscribtionsObserverManager futuresObserver)
		{
			_futuresObserver = futuresObserver;
		}

		[Authorize]
		public override async Task OrdersSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<OrdersChanged> responseStream, ServerCallContext context)
		{
			Guid subscribedId = Guid.Empty;
			try
			{
				await _futuresObserver.GetOrdersAsObservable(request.UserId, request.UserApiId, out var exchangedId, out subscribedId)
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
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				System.Diagnostics.Debug.WriteLine($"Orders subscribtion {subscribedId} was canceled.");
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Orders subscribtion {subscribedId} thrown an error: \n{ex}.");
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}

		[Authorize]
		public override async Task ValuesSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<ValuesChanged> responseStream, ServerCallContext context)
		{
			Guid subscribedId = Guid.Empty;
			try
			{
				await _futuresObserver.GetValuesAsObservable(request.UserId, request.UserApiId, out var exchangedId, out subscribedId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
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
				}, context.CancellationToken)
				.ConfigureAwait(false);
			}
			catch (TaskCanceledException)
			{
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				System.Diagnostics.Debug.WriteLine($"Trades subscribtion {subscribedId} was canceled.");
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Trades subscribtion {subscribedId} thrown an error: \n{ex}.");
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}

		[Authorize]
		public override async Task PositionsSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<PositionsChanged> responseStream, ServerCallContext context)
		{
			Guid subscribedId = Guid.Empty;
			try
			{
				await _futuresObserver.GetPositionsAsObservable(request.UserId, request.UserApiId, out var exchangedId, out subscribedId)
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
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				System.Diagnostics.Debug.WriteLine($"Positions subscribtion {subscribedId} was canceled.");
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Positions subscribtion {subscribedId} thrown an error: \n{ex}.");
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}

		[Authorize]
		public override async Task LeverageSubscribe(FuturesSubscribeRequest request, IServerStreamWriter<LeverageChanged> responseStream, ServerCallContext context)
		{
			Guid subscribedId = Guid.Empty;
			try
			{
				await _futuresObserver.GetLeveragesAsObservable(request.UserId, request.UserApiId, out var exchangedId, out subscribedId)
				.ToAsyncEnumerable()
				.ForEachAwaitAsync(async (x) =>
				{
					var orderChanged = new LeverageChanged
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
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				System.Diagnostics.Debug.WriteLine($"Leverages subscribtion {subscribedId} was canceled.");
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Leverages subscribtion {subscribedId} thrown an error: \n{ex}.");
				_futuresObserver.UnsubscribeIdAndTryToRemoveApiSubscribtionObject(subscribedId);
				throw;
			}
		}
	}
}
