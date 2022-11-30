using Ligric.Domain.SeedWork;
using System;

namespace Ligric.Domain.Customers.Orders.Events
{
    public class CustomerRemovedEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerRemovedEvent(Guid customerId)
        {
            this.CustomerId = customerId;
        }
    }
}