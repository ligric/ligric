using DevPace.Core.DataTypes;
using DevPace.Protos;
using System;

namespace DevPace.Core.GrpcClient
{
    public static class TypeExtensions
    {
        public static CustomersFilterRequest ToCustomersFilterRequest(this CustomersFilterDto customersFilter)
            => new CustomersFilterRequest() 
            { 
                CompanyName = customersFilter.CompanyName ?? string.Empty, 
                Email = customersFilter.Email ?? string.Empty,
                Flags = customersFilter.CustomersFilterFlags.ToString(), 
                Name = customersFilter.Name ?? string.Empty,
                Phone = customersFilter.Phone ?? string.Empty
            };

        public static CustomerDto ToCustomerDto(this Customer customer)
            => new CustomerDto(Guid.Parse(customer.Guid), customer.Name, customer.CompanyName, customer.Phone, customer.Email);
    }
}
