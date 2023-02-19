# How to avoid tight coupling

The whole reason we separate our application into components (microservices) is to avoid tight coupling and enable single responsibility. However, sometimes, these microservices are required to communicate with eachother to transmit information regarding changes in the application.

To achieve this we use an event bus. An Eventbus is a mechanism that allows different components to communicate with each other without knowing about each other. A component can send an Event to the Eventbus without knowing who will pick it up or how many others will pick it up.

Components can also listen to Events on an Eventbus, without knowing who sent the Events. That way, components can communicate without depending on each other. Also, it is very easy to substitute a component. As long as the new component understands the Events that are being sent and received, the other components will never know.

## Component Definition

So what exactly is a component here? Well, a component could be anything. In most Eventbuses, they are Java Objects. They send Events and they also listen to Events. In our case, a component is basically a microservice.

## Events Definition

And Events, what are they? Well, they are basically the messages that get sent and received by the components. Typically, they contain everything that the receiver needs to know in order to process the Event.

## Event Transmission

In order to transmit an event, we use an implementation of an Eventbus. The Eventbus takes a message and serializes it into a `Byte[]` array before transmitting it using a standard protocol. At the other end, when the message is received, it then needs to be deserialized into an object, in order for the microservice to process it.

In strict programming languages, this procedure would require the use of concrete object definitions being shared between the microservices, and this is how it was achieved in the past. A core library would be shared between the microservices allowing them to define the objects they would be using.

This process however goes agains the principles of DDD, and the whole reason why we would use microservices in the first place is to avoid such coupling. Another reason to consider why this practice is bad is to consider the possibility of different programming languages per microservice; there would be no way to share a common library.

## Overcoming object sharing between Microservices

The method we will be using in our application is to declare each event object individually per microservice. Most developers would dissagree with this approach as it would violate the DRY principles. However, DRY principles are only applied per microservice, as each component could be implemented in different programming languages, frameworks and/or versions.

Each microservice should have its own definitions, and update models according to domain requirements.
