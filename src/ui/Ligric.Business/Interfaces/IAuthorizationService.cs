﻿using Ligric.Core.Types.User;

namespace Ligric.Business.Interfaces
{
	public interface IAuthorizationService : ICurrentUser, IDisposable
	{
		event EventHandler<UserAuthorizationState>? AuthorizationStateChanged;

		Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct);

		Task SignUpAsync(string userName, string password, CancellationToken ct);

		Task SignInAsync(string userName, string password, CancellationToken ct);

		void Logout();
	}
}
