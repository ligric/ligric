using System.Collections.Generic;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Entities.UserApies;

namespace TemporaryProjectJustForCopyPast.Application.UserApis.GetUserApis
{
	public class GetAllowedApisCommand : CommandBase<IEnumerable<UserApiEntity>>
	{
		public long UserId { get; }

		public GetAllowedApisCommand(long userId)
		{
			UserId = userId;
		}
	}
}
