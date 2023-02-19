using System.Threading.Tasks;
using Microservices.Library.EventBus.Events;

namespace Microservices.Library.EventBus.Abstractions
{
    /// <summary>
    /// Generic contract of an integration event handler
    /// </summary>
    /// <typeparam name="TIntegrationEvent"></typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Public task that handles the integration event
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Handle(TIntegrationEvent @event);
    }

    /// <summary>
    /// Contracts an integration event handler
    /// </summary>
    public interface IIntegrationEventHandler { }
}
