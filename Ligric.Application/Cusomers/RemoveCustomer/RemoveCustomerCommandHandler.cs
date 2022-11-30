using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Customers;
using MediatR;
using Ligric.Application.Configuration.Data;
using Dapper;
using Ligric.Application.Customers;

namespace Ligric.Application.Orders.RemoveCustomer
{
    public class RemoveCustomerCommandHandler : ICommandHandler<RemoveCustomerCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;

        public RemoveCustomerCommandHandler(
             ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
        {
            await _customerRepository.RemoveAsync(request.CustomerId);
            return Unit.Value;
        }
    }
}