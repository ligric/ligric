using Ligric.Application.Configuration.Commands;
using Ligric.Application.Customers;

namespace Ligric.Application.Cusomers.AddCustomer
{
    public class AddCustomerCommand : CommandBase<CustomerDto>
    {
        public string Name { get; }

        public string CompanyName { get; }
        
        public string Phone { get; }

        public string Email { get; }

        public AddCustomerCommand(string name, string companyName, string phone, string email)
        {
            Name = name;
            CompanyName = companyName;
            Phone = phone;
            Email = email;
        }
    }
}
