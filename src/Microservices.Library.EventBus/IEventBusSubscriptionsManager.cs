using System;
using System.Collections.Generic;
using Microservices.Library.EventBus.Abstractions;
using Microservices.Library.EventBus.Events;
using static Microservices.Library.EventBus.InMemoryEventBusSubscriptionsManager;

namespace Microservices.Library.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// Public event that runs each time an event handler is removed
        /// </summary>
        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// Defines if the subscription manager contains any handlers
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Add a new subscription to a dynamic event
        /// </summary>
        /// <typeparam name="TH">The dynamic event handler</typeparam>
        /// <param name="eventName">The event name</param>
        void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Add a new subscription to a typed event
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <typeparam name="TH">The event handler type</typeparam>
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Removes a subscription based on event and handler type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <typeparam name="TH">The event handler type</typeparam>
        void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        /// <summary>
        /// Removes a subscription based on provided dynamic event handler type
        /// </summary>
        /// <typeparam name="TH">The dynamic event handler type</typeparam>
        /// <param name="eventName">The event name</param>
        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Checks if any  subscriptions exist for an event of provided type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>Boolean</returns>
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// Checks if any subscriptions exist for the provided event name
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <returns>Boolean</returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// Returns the type of an event by name
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <returns>Type</returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// Returns a list of subscribed handlers based on event type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>IEnumerable list of SubscriptionInfo</returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// Returns a list of subscribed handlers for a specific event by name
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <returns>IEnumerable list of SubscriptionInfo</returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        /// <summary>
        /// Returns the key of an event based on the event type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>String</returns>
        string GetEventKey<T>();

        /// <summary>
        /// Removes all the handlers from the subscription manager
        /// </summary>
        void Clear();
    }
}