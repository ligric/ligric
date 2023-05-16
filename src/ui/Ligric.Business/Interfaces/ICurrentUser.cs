using Ligric.Core.Types.User;

namespace Ligric.Business.Interfaces
{
	public interface ICurrentUser
	{
		UserDto? CurrentUser { get; }

		UserAuthorizationState CurrentConnectionState { get; }
	}
}
