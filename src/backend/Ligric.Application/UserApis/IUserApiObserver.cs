﻿using System;
using Ligric.Core.Ligric.Core.Types.Api;
using Utils;

namespace Ligric.Application.UserApis
{
	public interface IUserApiObserver
	{
		long Save(long apiId, string apiName, long userId, int permissions);

		void ShareToAll(long userApiId, int permissions);

		long Share(long userApiId, long sharedUserId, int permissions);

		IObservable<(EventAction Action, long UserId, ApiClientDto Api)> GetApisAsObservable(long userId);
	}
}