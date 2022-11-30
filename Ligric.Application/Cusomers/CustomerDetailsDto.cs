using System;

namespace Ligric.Application.Customers
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}