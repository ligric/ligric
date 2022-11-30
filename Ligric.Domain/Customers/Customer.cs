using System;
using Ligric.Domain.Customers.Events;
using Ligric.Domain.Customers.Orders.Events;
using Ligric.Domain.Customers.Rules;
using Ligric.Domain.SeedWork;

namespace Ligric.Domain.Customers
{
    public class Customer : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Name { get; private set; }

        public string CompanyName { get; private set; }

        public string Phone { get; private set; }

        public bool _isRemoved;

        private Customer()
        {

        }
         
        private Customer(Guid id, string email, string name, string companyName, string phone)
        {
            this.Id = id;
            Email = email;
            Name = name;
            CompanyName = companyName;
            Phone = phone;

            this.AddDomainEvent(new CustomerPlacedEvent(this.Id));
        }

        public static Customer Create(
            string email, 
            string name, 
            string companyName, 
            string phone,
            IUniqueIdGenerator idGenerator,
            ICustomerUniquenessChecker customerUniquenessChecker)
        {
            CheckRule(new CustomerEmailMustBeUniqueRule(customerUniquenessChecker, email));

            Guid id = idGenerator.GetUniqueId();

            return new Customer(id, email, name, companyName, phone);
        }

        public void Change(
            string email,
            string name,
            string companyName,
            string phone,
            ICustomerUniquenessChecker customerUniquenessChecker)
        {
            CheckRule(new CustomerEmailMustBeUniqueRule(customerUniquenessChecker, email));

            Email = email;
            Name = name;
            CompanyName = companyName;
            Phone = phone;

            this.AddDomainEvent(new CustomerChangedEvent(this.Id));
        }

        public void Remove()
        {
            this._isRemoved = true;
            this.AddDomainEvent(new CustomerRemovedEvent(this.Id));
        }
    }
}