using System.Threading;
using System.Threading.Tasks;
using Ligric.Server.Application.Configuration.Commands;
using System;
using Ligric.Server.Domain.Entities.UserApies;
using System.Collections.Generic;

namespace Ligric.Server.Application.UserApis.GetUserApis
{
	public class GetAllowedApisCommandHandler : ICommandHandler<GetAllowedApisCommand, IEnumerable<UserApiEntity>>
    {
        public GetAllowedApisCommandHandler()
        {

        }

        public async Task<IEnumerable<UserApiEntity>> Handle(GetAllowedApisCommand request, CancellationToken cancellationToken)
        {
			throw new NotImplementedException();
        }
    }
}
