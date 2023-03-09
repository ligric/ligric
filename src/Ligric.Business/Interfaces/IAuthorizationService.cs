﻿using Ligric.Core.Types.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ligric.Business.Authorization;

public interface IAuthorizationService : IDisposable
{
	UserDto CurrentUser { get; }

	UserAuthorizationState CurrentConnectionState { get; }

	event EventHandler<UserAuthorizationState> AuthorizationStateChanged;

	Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct);

	Task SignUpAsync(string userName, string password, CancellationToken ct);

	Task SignInAsync(string userName, string password, CancellationToken ct);
}
