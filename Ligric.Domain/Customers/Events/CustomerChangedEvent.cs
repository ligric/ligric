using Ligric.Domain.SeedWork;
using System;

namespace Ligric.Domain.Customers.Events
{
    public class CustomerChangedEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerChangedEvent(Guid customerId)
        {
            this.CustomerId = customerId;
        }
    }
}