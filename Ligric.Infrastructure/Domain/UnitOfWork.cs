using System.Threading;
using System.Threading.Tasks;
using Ligric.Domain.SeedWork;
using Ligric.Infrastructure.Database;
using Ligric.Infrastructure.Processing;

namespace Ligric.Infrastructure.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DevPaceContext _devPaceContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(
            DevPaceContext devPaceContext, 
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this._devPaceContext = devPaceContext;
            this._domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this._domainEventsDispatcher.DispatchEventsAsync();
            return await this._devPaceContext.SaveChangesAsync(cancellationToken);
        }
    }
}