using System.Threading.Tasks;
using Autofac;
using MediatR;
using Ligric.Server.Application;
using Ligric.Server.Application.Configuration.Queries;

namespace Ligric.Server.Infrastructure.Processing
{
    public static class QueriesExecutor
    {
        public static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using (var scope = CompositionRoot.BeginLifetimeScope())
            {
				if (scope == null)
                {
                    throw new System.NotImplementedException();
                }
                var mediator = scope.Resolve<IMediator>();
                return await mediator.Send(query);
            }
        }
    }
}
