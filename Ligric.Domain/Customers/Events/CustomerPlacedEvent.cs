using Ligric.Domain.SeedWork;
using System;

namespace Ligric.Domain.Customers.Events
{
    public class CustomerPlacedEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerPlacedEvent(Guid customerId)
        {
            this.CustomerId = customerId;
        }
    }
}