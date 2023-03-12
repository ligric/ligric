using System;

namespace Ligric.Service.AuthService.Domain.Events.DataTypes
{
	public record UserCreatedOrUpdatedData(string Id, string UniqueName, DateTime CreatedDate)
	{
		public UserCreatedOrUpdatedData() : this(string.Empty, string.Empty, default)
		{
		}
	}
}
