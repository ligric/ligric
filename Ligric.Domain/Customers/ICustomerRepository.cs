using System;
using System.Threading.Tasks;

namespace Ligric.Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid id);

        Task AddAsync(Customer customer);

        Task RemoveAsync(Guid id);
    }
}