using System.Collections.Generic;
using Ligric.Application.Configuration.Commands;
using Ligric.Server.Domain.Entities.UserApies;

namespace Ligric.Application.UserApis.GetUserApis
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
