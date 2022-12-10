using System;
using Utils.DtoTypes;

namespace DevPace.Core.DataTypes
{
    public class CustomerDto : GuidDto
    {
        public string Name { get; }

        public string CompanyName { get; }

        public string Phone { get; }

        public string Email { get; }

        public CustomerDto(Guid id, string name, string companyName, string phone, string email)
            :base(id)
        {
            Name = name;
            CompanyName = companyName;
            Phone = phone;
            Email = email ?? throw new ArgumentNullException("Email cannot be null.");
        }

        protected override bool EqualsCore(GuidDto dto)
        {
            return dto is CustomerDto other &&
                Equals(other.Name, Name) &&
                Equals(other.CompanyName, CompanyName) &&
                Equals(other.Phone, Phone) &&
                Equals(other.Email, Email);
        }
    }
}
