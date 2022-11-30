using System;
using Ligric.Application.Configuration.Commands;
using MediatR;

namespace Ligric.Application.Orders.RemoveCustomer
{
    public class RemoveCustomerCommand : CommandBase<Unit>
    {
        public Guid CustomerId { get; }

        public RemoveCustomerCommand(Guid customerId)
        {
            this.CustomerId = customerId;
        }
    }
}