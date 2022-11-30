using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ligric.Infrastructure.Database;
using Ligric.Domain.Customers;

namespace Ligric.Infrastructure.Domain.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DevPaceContext _context;

        public CustomerRepository(DevPaceContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Customer customer)
        {
            await this._context.Customers.AddAsync(customer);
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await this._context.Customers.SingleAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            var customer = await this._context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            this._context.Customers.Remove(customer);
        }
    }
}