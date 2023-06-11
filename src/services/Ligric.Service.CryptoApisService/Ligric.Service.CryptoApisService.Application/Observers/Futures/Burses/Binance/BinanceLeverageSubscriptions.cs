using System.Reactive.Linq;
using Ligric.Service.CryptoApisService.Application.Observers.Futures.Interfaces;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Extensions;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.Observers.Futures.Burses.Binance
{
	public class BinanceLeverageSubscriptions : ILeverageSubscriptions
	{
		private readonly IApiRepository _apiRepository;
		private readonly BinanceFuturesApiSubscriptions _futuresApiSubscriptions;
		private event Action<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)>? laveragesChanged;

		public BinanceLeverageSubscriptions(
			BinanceFuturesApiSubscriptions futuresApiSubscriptions,
			IApiRepository apiRepository)
		{
			_apiRepository = apiRepository;
			_futuresApiSubscriptions = futuresApiSubscriptions;
		}

		public IObservable<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)> GetLeveragesAsObservable()
		{
			var updatedApiStateNotifications = Observable.FromEvent<(Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs)>((x)
				=> laveragesChanged += x, (x) => laveragesChanged -= x);

			return updatedApiStateNotifications;
		}

		public void SetSubscribedStream(long userApiId, long userId, out Guid subscribedStreamId, out Guid chainSessionId)
		{
			var api = _apiRepository.GetEntityByUserApiId(userApiId).ToApiDto();

			_futuresApiSubscriptions.AttachSubscriptionIdToApi(api, userId, out subscribedStreamId, out var chainSession);
			chainSessionId = chainSession.BurseSessionId;
			chainSession.LeveragesChanged += OnLeveragesChanged;
		}

		public void UnsubscribeStream(Guid subscribedStreamId)
		{
			_futuresApiSubscriptions.DetachSubscriptionAndTryToRemoveApiSubscriptionObject(subscribedStreamId);
		}

		private void OnLeveragesChanged((Guid ExchangeId, NotifyDictionaryChangedEventArgs<string, byte> EventArgs) obj)
			=> laveragesChanged?.Invoke(obj);
	}
}
