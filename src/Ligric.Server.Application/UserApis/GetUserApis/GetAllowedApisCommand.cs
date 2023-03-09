using System.Collections.Generic;
using Ligric.Server.Application.Configuration.Commands;
using Ligric.Server.Domain.Entities.UserApies;

namespace Ligric.Server.Application.UserApis.GetUserApis
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
