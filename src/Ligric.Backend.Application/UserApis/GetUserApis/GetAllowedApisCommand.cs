using System.Collections.Generic;
using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Backend.Domain.Entities.UserApies;

namespace Ligric.Backend.Application.UserApis.GetUserApis
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
