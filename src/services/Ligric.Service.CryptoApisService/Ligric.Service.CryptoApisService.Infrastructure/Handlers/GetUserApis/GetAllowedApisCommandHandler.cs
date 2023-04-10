using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Infrastructure.Persistence.Commands;
using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Infrastructure.Handlers.GetUserApis
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
