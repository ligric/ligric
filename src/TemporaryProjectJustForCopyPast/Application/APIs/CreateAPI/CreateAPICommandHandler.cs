using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using System;

namespace TemporaryProjectJustForCopyPast.Application.APIs.CreateAPI
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
