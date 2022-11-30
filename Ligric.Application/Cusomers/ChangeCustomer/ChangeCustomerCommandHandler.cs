using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Customers;
using MediatR;

namespace Ligric.Application.Customers.ChangeCustomer
{
    public class ChangeCustomerCommandHandler : ICommandHandler<ChangeCustomerCommand, Unit>
    {
        private readonly ICustomerUniquenessChecker _customerUniquenessChecker;

        public ChangeCustomerCommandHandler(
            ICustomerUniquenessChecker customerUniquenessChecker)
        {
            this._customerUniquenessChecker = customerUniquenessChecker;
        }

        public async Task<Unit> Handle(ChangeCustomerCommand request, CancellationToken cancellationToken)
        {
         
            return Unit.Value;
        }
    }
}