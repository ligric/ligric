using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Ligric.Application.Configuration.Data;
using Ligric.Application.Configuration.Queries;

namespace Ligric.Application.Customers.GetCustomerDetails
{
    public class GetCustomerDetailsQueryHandler : IQueryHandler<GetCustomerDetailsQuery, CustomerDetailsDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetCustomerDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public Task<CustomerDetailsDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
            const string sql = "SELECT " +
                               "[Customer].[Id], " +
                               "[Customer].[Email], " +
                               "[Customer].[Name], " +
                               "[Customer].[CompanyName], " +
                               "[Customer].[Phone] " +
                               "FROM devPace.v_Customers AS [Customer] " +
                               "WHERE [Customer].[Id] = @Id "; //CustomerId

            var connection = _sqlConnectionFactory.GetOpenConnection();

            return connection.QuerySingleAsync<CustomerDetailsDto>(sql, new
            {
                request.CustomerId
            });
        }
    }
}