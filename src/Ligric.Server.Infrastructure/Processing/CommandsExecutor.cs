using System.Threading.Tasks;
using Autofac;
using MediatR;
using Ligric.Server.Application.Configuration.Commands;

namespace Ligric.Server.Infrastructure.Processing
{
    public static class CommandsExecutor
    {
        public static async Task Execute(ICommand command)
        {
            using (var scope = CompositionRoot.BeginLifetimeScope())
            {
				if (scope == null)
				{
					throw new System.NotImplementedException();
				}
				var mediator = scope.Resolve<IMediator>();
				await mediator.Send(command);
			}
		}

        public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using (var scope = CompositionRoot.BeginLifetimeScope())
            {
				if (scope == null)
				{
					throw new System.NotImplementedException();
				}
				var mediator = scope.Resolve<IMediator>();
                return await mediator.Send(command);
            }
        }
    }
}
