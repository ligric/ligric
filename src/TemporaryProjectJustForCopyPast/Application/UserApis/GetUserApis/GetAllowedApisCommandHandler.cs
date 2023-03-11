using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using System;
using Ligric.Domain.Entities.UserApies;
using System.Collections.Generic;

namespace TemporaryProjectJustForCopyPast.Application.UserApis.GetUserApis
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
