using System.Threading;
using System.Threading.Tasks;
using Ligric.Backend.Application.Configuration.Commands;
using System;
using Ligric.Backend.Domain.Entities.UserApies;
using System.Collections.Generic;

namespace Ligric.Backend.Application.UserApis.GetUserApis
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
