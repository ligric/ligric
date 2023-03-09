using System.Threading;
using System.Threading.Tasks;
using Ligric.Server.Application.Configuration.Commands;
using System;

namespace Ligric.Server.Application.Users.RegisterUser
{
	public class CreateAPICommandHandler : ICommandHandler<CreateAPICommand, int>
    {
        public CreateAPICommandHandler()
        {
        }

        public async Task<int> Handle(CreateAPICommand request, CancellationToken cancellationToken)
        {
			throw new NotImplementedException();
        }
    }
}
