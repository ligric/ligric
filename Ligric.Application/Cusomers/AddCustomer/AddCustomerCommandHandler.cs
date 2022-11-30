using Ligric.Domain.Customers;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.Customers;
using System.Threading.Tasks;
using System.Threading;
using Ligric.Domain.SeedWork;
using System;

namespace Ligric.Application.Cusomers.AddCustomer
{
    public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUniqueIdGenerator _idGenerator;
        private readonly ICustomerUniquenessChecker _customerUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        internal AddCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUniqueIdGenerator idGenerator,
            ICustomerUniquenessChecker customerUniquenessChecker,
            IUnitOfWork unitOfWork)
        {
            this._customerRepository = customerRepository;
            this._idGenerator = idGenerator;
            this._customerUniquenessChecker = customerUniquenessChecker;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CustomerDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = Customer.Create(request.Email, request.Name, request.CompanyName, request.Phone, this._idGenerator, this._customerUniquenessChecker);

            await this._customerRepository.AddAsync(customer);

            await this._unitOfWork.CommitAsync(cancellationToken);

            return new CustomerDto { Id = customer.Id };
        }
    }
}
