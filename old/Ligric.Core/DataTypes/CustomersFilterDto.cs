using System;
using System.Collections.Generic;
using System.Text;

namespace DevPace.Core.DataTypes
{
    [Flags]
    public enum CustomersFilterFlags
    {
        //
        // Summary:
        //     Specifies that no binding flags are defined.
        Default = 0,
        //
        // Summary:
        //     Specifies that the case of the member Name should be unique
        UniqueName = 1,

        // Summary:
        //     Specifies that the case of the member Email should be unique
        UniqueEmail = 2,

        // Summary:
        //     Specifies that the case of the member Phone should be unique
        UniquePhone = 4,

        // Summary:
        //     Specifies that the case of the member Companyname should be unique
        UniqueCompanyName = 8,
    }

    public class CustomersFilterDto
    {
        public string Name { get; }

        public string CompanyName { get; }

        public string Phone { get; }

        public string Email { get; }

        public CustomersFilterFlags CustomersFilterFlags { get; }

        public CustomersFilterDto(string name, string companyName, string phone, string email, CustomersFilterFlags flags)
        {
            Name = name;
            CompanyName = companyName;
            Phone = phone;
            Email = email;
            CustomersFilterFlags = flags;
        }
    }
}
