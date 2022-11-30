using Dapper;
using Ligric.Application.Configuration.Data;
using Ligric.Domain.Customers;

namespace Ligric.Application.Cusomers.DomainServices
{
    public class CustomerUniquenessChecker : ICustomerUniquenessChecker
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CustomerUniquenessChecker(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public bool IsUnique(string customerEmail)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT TOP 1 1" +
                               "FROM [devPace].[Customers] AS [Customer] " +
                               "WHERE [Customer].[Email] = @Email";
            var customersNumber = connection.QuerySingleOrDefault<int?>(sql,
                            new
                            {
                                Email = customerEmail
                            });

            return !customersNumber.HasValue;
        }
    }
}
