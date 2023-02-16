using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using System;

namespace Ligric.Application.UserApis.CreateUserApi
{
	public class CreateUserApiCommandHandler : ICommandHandler<CreateUserApiCommand, bool>
    {
        public CreateUserApiCommandHandler()
        {
        }

        public async Task<bool> Handle(CreateUserApiCommand request, CancellationToken cancellationToken)
        {
			throw new NotImplementedException();
        }
    }
}
