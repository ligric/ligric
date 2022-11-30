namespace Ligric.Application.Users.LoginCustomer
{
    //public class LoginCustomerCommandHandler : ICommandHandler<LoginCustomerCommand, CustomerDto>
    //{
    //    private readonly IUserRepository _customerRepository;
    //    private readonly IUserUniquenessChecker _customerUniquenessChecker;
    //    private readonly IUnitOfWork _unitOfWork;

    //    public LoginCustomerCommandHandler(
    //        IUserRepository customerRepository, 
    //        IUserUniquenessChecker customerUniquenessChecker, 
    //        IUnitOfWork unitOfWork)
    //    {
    //        this._customerRepository = customerRepository;
    //        _customerUniquenessChecker = customerUniquenessChecker;
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<CustomerDto> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
    //    {
    //        var customer = User.(request.Name, request.CompanyName, request.Phone, request.Email, this._customerUniquenessChecker);

    //        await this._customerRepository.AddAsync(customer);

    //        await this._unitOfWork.CommitAsync(cancellationToken);

    //        return new CustomerDto { Id = customer.Id.Value };
    //    }
    //}
}