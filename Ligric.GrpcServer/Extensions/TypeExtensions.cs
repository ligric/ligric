using Ligric.Application.Customers;
using Ligric.Protos;

namespace Ligric.GrpcServer.Extensions
{
    public static class TypeExtensions
    {
        public static Customer ToCustomer(this CustomerDetailsDto customerDetails)
        {
            return new Customer()
            {
                Guid = customerDetails.Id.ToString(),
                Email = customerDetails.Email,
                Name = customerDetails.Name,
                CompanyName = customerDetails.CompanyName,
                Phone = customerDetails.Phone
            };
        }
    }
}
