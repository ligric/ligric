using System.Threading;
using System.Threading.Tasks;
using Ligric.Service.CryptoApisService.Infrastructure.Processing;
using MediatR;

namespace Ligric.Infrastructure.Processing
{
	public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
    {
        private readonly INotificationHandler<T> _decorated;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher, 
            INotificationHandler<T> decorated)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _decorated = decorated;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(notification, cancellationToken);

            await this._domainEventsDispatcher.DispatchEventsAsync();
        }
    }
}
