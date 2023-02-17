using System;
using Ligric.Domain.Types.Api;
using Utils;

namespace Ligric.Application.UserApis
{
	public interface IUserApiObserver
	{
		IObservable<(EventAction Action, ApiClientDto apiClient)> GetUserApiAsObservable(long userId);
	}
}
