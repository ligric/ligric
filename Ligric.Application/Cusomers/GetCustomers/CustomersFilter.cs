using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Ligric.Application.Cusomers.GetCustomers
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

    public class CustomersFilter
    {
        public string? Name { get; }

        public string? CompanyName { get; }

        public string? Phone { get; }

        public string? Email { get; }

        public CustomersFilterFlags CustomersFilterFlags { get; }

        // --------------------------------------
        // TODO : HARD CODE
        // Should be flag rules.
        // --------------------------------------
        public CustomersFilter(
            CustomersFilterFlags flags,
            string? name = null, 
            string? companyName = null, 
            string? phone = null, 
            string? email = null)
        {
            Name = name;
            CompanyName = companyName;
            Phone = phone;
            Email = email;
            CustomersFilterFlags = flags;
        }
    }

    public static class CustomersFilterFlagsExtensions
    {
        /// <summary>
        /// Extesion for sql "=" or "like" mode
        /// </summary>
        /// <param name="filterFlags"></param>
        /// <param name="value">Property value. Should be like "example.@gmail.com"</param>
        /// <returns>if flag is contained then it will be "= 'Email'", else "like m%Email%'"</returns>
        public static string SqlUniqueFromContains(this IEnumerable<CustomersFilterFlags> filterFlags, CustomersFilterFlags filterFlag, string value)
        {
            return filterFlags.Contains(filterFlag) ? $"= '{value}'" : $"like '%{value}%'";
        }
    }
}
