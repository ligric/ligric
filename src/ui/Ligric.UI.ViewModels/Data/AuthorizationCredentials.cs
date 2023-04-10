using System;
using System.Collections.Generic;
using System.Text;

namespace Ligric.UI.ViewModels.Data
{
	public enum AutorizationMode
	{
		SignIn,
		SignUp
	}

	public record AuthorizationCredentials
	{
		public AutorizationMode AutorizationMode { get; init; }

		public string? UserName { get; init; }

		public string? Password { get; init; }

		public string? RepeatedPassword { get; init; }
	}
}
