using System;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;
using Utils;

namespace Ligric.Service.CryptoApisService.Application.TemporaryObservers
{
	public interface IUserApiObserver
	{
		long Save(long apiId, string apiName, long userId, int permissions);

		void ShareToAll(long userApiId, int permissions);

		long Share(long userApiId, long sharedUserId, int permissions);

		IObservable<(EventAction Action, ApiClientResponseDto Api)> GetApisAsObservable(long userId);
	}
}
