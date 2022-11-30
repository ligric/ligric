using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Queries;
using Ligric.Domain.Customers;

namespace Ligric.Application.Customers.CheckCustomerEmailExists
{
    public class CustomerEmailIsUniqueQueryHandler : IQueryHandler<CustomerEmailIsUniqueQuery, bool>
    {
        private readonly ICustomerUniquenessChecker _customerUniquenessChecker;

        public CustomerEmailIsUniqueQueryHandler(
            ICustomerUniquenessChecker customerUniquenessChecker)
        {
            this._customerUniquenessChecker = customerUniquenessChecker;
        }

        public Task<bool> Handle(CustomerEmailIsUniqueQuery request, CancellationToken cancellationToken)
        {
            bool isEmailUnique = this._customerUniquenessChecker.IsUnique(request.Email);
            return Task.FromResult(isEmailUnique);
        }
    }
}