# Project Details

The concept of an event bus may be new to you, but you may be familiar with the observer (publish-subscribe) model. The event bus is an implementation of the publish-and-subscribe model. It is a centralized event handling mechanism that allows different components to communicate with each other without interdependence, for a decoupling purpose.

![Figure 1](/.media/b26e5f214d72f3b0a49199d9e5c41652.png)

As you can see from the above figure, the core has four roles:

1. Event (event source + event processing)
1. Event publisher
1. Event subscriber
1. Event bus

The key to implementing an event bus is:

1. The event bus maintains a mapping dictionary of event sources and event processing;
1. Ensure the unique entry of the event bus through the singleton mode;
1. Use reflection to complete the initialization binding of event source and event processing;
1. Provides unified event registration, unregistration, and triggering interfaces.

We look directly at the realization mechanism and the class diagram from the perspective of a God.

![Figure 2](/.media/530e69dfffbe664ffd7008782d456b33.png)

We know that the essence of the event is: **Event source + Event processing**.

## Event Source

For the event source, which is defined as an `IntegrationEvent` base class to be handled. By default, only one `Guid` and one creation date are included. Specific events can be used to improve the description of the event by inheriting the class.

>It is necessary to explain here the `IntegrationEvent`. Because the consumption of events in microservice is no longer limited to the current domain, but multiple microservices may share the same event, it is distinguished from the domain events in the DDD. Integration events can be used to synchronize domain state across multiple microservices or external systems by publishing integration events outside of microservices.

## Event Processing

For event processing, its essence is the reaction to the event. An event can cause multiple reactions, so there is a one-to-many relationship between them. In this project we abstract two different interfaces for event handling:

1. `IIntegrationEventHandler`
1. `IDynamicIntegrationEventHandler`

Both define a `Handle` method which is used to respond to events. The difference is in the type of method parameter:

- The first one accepted was a strong type `IntegrationEvent`.
- The second one is a dynamic type `dynamic`

### Why should I provide a separate event source for dynamic? What about the type of interface?

Not every event source requires detailed event information, so a strongly typed parameter constraint is not necessary. It can simplify the construction of event sources and become more flexible.

With event sources and event handling, the next step is the registration and subscription of events. In order to facilitate subscription management, the system provides an additional layer of abstraction `IEventBusSubscriptionsManager`. 

It is used to maintain subscription and logout of events, as well as persistence of subscription information. Its default implementation `InMemoryEventBusSubscriptionsManager`, is a mapping dictionary that uses memory for storing event sources and event processing.

Seen from the class diagram that in `InMemoryEventBusSubscriptionsManager`, an inner class is defined called `SubscriptionInfo`, which is mainly used to indicate the subscription type of the event subscriber and the type of event processing.