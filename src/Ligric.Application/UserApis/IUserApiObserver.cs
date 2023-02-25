using System;
using System.Collections.Generic;
using System.Text;
using Ligric.Domain.Types.Api;
using Utils;

namespace Ligric.Application.UserApis
{
	public interface IUserApiObserver
	{
		long Save(long apiId, long userId, int permissions);

		IObservable<(EventAction Action, long UserId, ApiClientDto Api)> GetApisAsObservable(long userId);
	}
}
