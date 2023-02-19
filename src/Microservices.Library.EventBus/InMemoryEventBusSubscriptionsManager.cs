using System;
using System.Collections.Generic;
using System.Linq;

using Microservices.Library.EventBus;
using Microservices.Library.EventBus.Abstractions;
using Microservices.Library.EventBus.Events;

namespace Microservices.Library.EventBus
{
    /// <summary>
    /// An in memory event bus subscription manager
    /// </summary>
    public partial class InMemoryEventBusSubscriptionsManager
        : IEventBusSubscriptionsManager
    {
        // A dictionary that holds a list of handlers by event name
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        // A list of event types supported
        private readonly List<Type> _eventTypes;

        /// <summary>
        /// Public event that runs each time an event handler is removed
        /// </summary>
        public event EventHandler<string>? OnEventRemoved;

        /// <summary>
        /// Defines if the subscription manager contains any handlers
        /// </summary>
        public bool IsEmpty => !_handlers.Keys.Any();

        /*
         * CONSTRUCTORS
         */

        /// <summary>
        /// Creates a new instance of an in memory event bus subscription manager
        /// </summary>
        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        /// <summary>
        /// Add a new subscription to a dynamic event
        /// </summary>
        /// <typeparam name="TH">The dynamic event handler</typeparam>
        /// <param name="eventName">The event name</param>
        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        /// <summary>
        /// Add a new subscription to a typed event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        /// <summary>
        /// Removes a subscription based on event and handler type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <typeparam name="TH">The event handler type</typeparam>
        public void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>() ?? throw new NullReferenceException();
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// Removes a subscription based on provided dynamic event handler type
        /// </summary>
        /// <typeparam name="TH">The dynamic event handler type</typeparam>
        /// <param name="eventName">The event name</param>
        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName) ?? throw new NullReferenceException();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// Checks if any  subscriptions exist for an event of provided type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>Boolean</returns>
        public bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        /// <summary>
        /// Checks if any subscriptions exist for the provided event name
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <returns>Boolean</returns>
        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        /// <summary>
        /// Returns the type of an event by name
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <returns>Type</returns>
        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        /// <summary>
        /// Returns a list of subscribed handlers based on event type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>IEnumerable list of SubscriptionInfo</returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        /// <summary>
        /// Returns a list of subscribed handlers for a specific event by name
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <returns>IEnumerable list of SubscriptionInfo</returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        /// <summary>
        /// Returns the key of an event based on the event type
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>String</returns>
        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// Removes all the handlers from the subscription manager
        /// </summary>
        public void Clear() => _handlers.Clear();


        /*
         * PRIVATE FUNCTIONS
         */

        // Adds a new subscription to a typed or dynamic event
        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        // Removes an existing handler
        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }

            }
        }

        // Invokes the on event removed event
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        // Finds the subscription information of an event to remove based on dynamic event handler type
        private SubscriptionInfo? FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        // Finds the subscription information of an event to remove based on specific event and event handler types
        private SubscriptionInfo? FindSubscriptionToRemove<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        // Finds the subscription information of an event to remove based on the event name and handler type
        private SubscriptionInfo? DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

        }
    }
}
