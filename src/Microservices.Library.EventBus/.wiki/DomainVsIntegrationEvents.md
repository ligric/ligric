# Domain Events vs. Integration Events

When discussing about “Domain Events” there’s certain lack of clarity because sometimes you can be talking about Domain Events that happened but within the same transaction scope of the operation that is still in memory objects (so I fact, it still didn’t happen from a persistence point of view) or, on the other hand, you could be talking about events that were already committed in your persistence storage and must be published because of state propagation and consistency between multiple microservices, bounded-contexts or external systems. In the latter case, it really has happened in the past, from an application and persistence point of view.

The latter type of events can be used as “integration events” because its related data was persisted for sure, therefore, you could use any implementation of a EventBus.Publish() and publish asynchronously that event to any subscriber whether the subscriber is in-memory or is remote and out-of-process and you are communicating through a real messaging system like a Service Bus or any queue based system that supports a Pubs/Subs model.

![Domain vs. Integration Events](/.media/image_thumb98.png)

## Domain Events

That mentioned difference is really important because in the first model (pure Domain Events) you must do everything in-process (in-memory) and within the same transaction or operation. You have to raise the event from a business method in a Domain Entity class (An AggregateRoot or a Child Domain Entity) and handle the event from multiple handlers (anyone subscribed) but all happening within the same transaction scope. 

That is critical because if when publishing a domain event, that event was published asynchronously in a “fire and forget” way to somewhere else, including the possibility of remote systems, that event propagation would have occurred before committing your original transaction, therefore, if your original transaction or operation fails, you will have inconsistent data between your original local domain and the subscribed systems (potentially, out-of-process, like another microservice).

However, if those events are handled only within the same transaction scope and in-memory, then, the approach is good and provides a “fully encapsulated Domain Model”. In short, in-memory Domain events and their side effects (event handler’s operations) need to occur within the same logical transaction.

![Test](/.media/image_thumb732.png)

In the figure above we see how domaine events remain within the bounded context of their own microservice and concert one aggregate.

### Two-phase Domain events (a la Jimmy Bogard)

> I called this “Two phase” domain events because clearly is has two decoupled phases. But it has nothing to do with “Two Phase Commit Transactions based on the DTC, etc..”…

So, Jimmy Bogard wrote an interesting blog post ([A better Domain Events pattern](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/)) where he describes an alternate method to work with Domain Events where it still means event side effect operations within the same transaction scope or group of operations in-memory, but using an improved way which decouples the raise event operation from the dispatch operation.

He starts highlighting the issues of the original approach saying that the DomainEvents class dispatches to handlers immediately. That makes testing your domain model difficult, because side effects (operations executed by event handlers) are executed immediately at the point of raising the domain event.

But, what if instead of directly raising the domain events from the domain entities (remember that in that case the events will be handled immediately) you were recording the domain events happening, and before committing the transaction, you dispatch all those domain events at that point?

You can see now that from within the entity class you are not raising/publishing domain events but only adding events to a collection within the same scope. The Events collection would be a child collection of events per Domain Entity that needs events (usually AggregateRoots are the entities that need events, most of all).

Finally, you need to actually fire off these domain events. That is something that we already hooked into our Entity Framework context class under the SaveChanges method, as the following:

```C#
public override int SaveChanges() {
    var domainEventEntities = ChangeTracker.Entries<IEntity>()
    .Select(po => po.Entity)
    .Where(po => po.Events.Any())
    .ToArray();
    foreach (var entity in domainEventEntities)
    {
        var events = entity.Events.ToArray();
        entity.Events.Clear();
        foreach (var domainEvent in events)
        {
            _dispatcher.Dispatch(domainEvent);
        }
    }

    return base.SaveChanges();
}
```

In any case you can see that you’ll be dispatching the events right before committing the original transactional operations using EF DbContext SaveChanges(). That is why those events and their event-handlers still need to be running in-memory and as part of the same transaction. Other than that, if you were sending remote and asynchronous events (like integration events) but “SaveChanges()” fails, you would have inconsistent data between your multiple models, microservices or external systems.

You could use this approach for messaging (out-of-proc communication) only in the case where you are publishing/dispatching messages through some transactional artifact, like a transactional queue, so if your original .SaveChanges() fails you would take the event-message back from the transactional queue before it is processed by other microservices or external systems.

## Integration Events

Dealing with integration events is usually a different matter than using domain events within the same transaction. You might need to send asynchronous events to communicate and propagate changes from one original domain model (the original microservice or original bounded-context, for instance) to multiple subscribed microservices or even external subscribed applications.

Because of that asynchronous communication, you should not publish integration events until you are 100% sure that your original operation, the event, really happened in the past. That means it was persisted in a database or any durable system. If you don’t do that, you can end up with inconsistent information across multiple microservices, bounded-contexts or external applications.

This kind of Integration Event is pretty similar to the events used in a full CQRS with a different “reads-database” than the “writes-database” and you need to propagate changes/events from the transactional or “writes-database” to the “reads-database”.

A very similar scenario would be when communicating out-of-process microservices, each microservice owning its own model, as it is really the same pattern, as well.

Therefore, when you publish an integration event you usually would do it from the infrastructure layer or the application layer, right after saving the data to the persistent database.

## References

- [Domain Events vs. Integration Events in Domain-Driven Design and microservices architectures](https://devblogs.microsoft.com/cesardelatorre/domain-events-vs-integration-events-in-domain-driven-design-and-microservices-architectures/)
