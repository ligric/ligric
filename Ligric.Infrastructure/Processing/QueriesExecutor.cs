using System.Threading.Tasks;
using Autofac;
using MediatR;
using Ligric.Application;
using Ligric.Application.Configuration.Queries;

namespace Ligric.Infrastructure.Processing
{
    public static class QueriesExecutor
    {
        public static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using (var scope = CompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}