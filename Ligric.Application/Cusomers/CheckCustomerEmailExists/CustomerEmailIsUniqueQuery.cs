using Ligric.Application.Configuration.Queries;

namespace Ligric.Application.Customers.CheckCustomerEmailExists
{
    public class CustomerEmailIsUniqueQuery : IQuery<bool>
    {
        public string Email { get; }

        public CustomerEmailIsUniqueQuery(string login)
        {
            Email = login;
        }
    }
}
