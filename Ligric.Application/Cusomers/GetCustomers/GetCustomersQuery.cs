using Ligric.Application.Configuration.Queries;
using Ligric.Application.Cusomers.GetCustomers;
using System.Collections.Generic;

namespace Ligric.Application.Customers.GetCustomers
{
    public class GetCustomersQuery : IQuery<IEnumerable<CustomerDetailsDto>>
    {
        public CustomersFilter? Filter { get; }

        public GetCustomersQuery(CustomersFilter? filter)
        {
            Filter = filter;
        }
    }
}