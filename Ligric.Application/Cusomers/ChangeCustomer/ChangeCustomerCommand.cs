using System;
using Ligric.Application.Configuration.Commands;
using MediatR;

namespace Ligric.Application.Customers.ChangeCustomer
{
    public class ChangeCustomerCommand : CommandBase<Unit>
    {
        public Guid CustomerId { get; }

        public string Email { get; }

        public string Name { get; }

        public string CompanyName { get; }

        public string Phone { get; }

        public ChangeCustomerCommand(Guid id, string email, string name, string companyName, string phone)
        {
            CustomerId = id;
            Email = email;
            Name = name;
            CompanyName = companyName;
            Phone = phone;
        }
    }
}