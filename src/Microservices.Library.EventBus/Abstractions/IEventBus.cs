using Microservices.Library.EventBus.Events;

namespace Microservices.Library.EventBus.Abstractions
{
    /// <summary>
    /// Contracts an event bus
    /// </summary>
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// Subscribes to an event
        /// </summary>
        /// <typeparam name="T">The integration event type</typeparam>
        /// <typeparam name="TH">The integration event handler type</typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Subscribe to a dynamic event
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Unsubscribe from dynamic event
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Unsubscribe from event
        /// </summary>
        /// <typeparam name="T">The integration event type</typeparam>
        /// <typeparam name="TH">The integration event handler type</typeparam>
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
