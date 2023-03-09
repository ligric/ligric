using System.Threading;
using System.Threading.Tasks;
using Ligric.Backend.Application.Configuration.Commands;
using System;

namespace Ligric.Backend.Application.Users.RegisterUser
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
