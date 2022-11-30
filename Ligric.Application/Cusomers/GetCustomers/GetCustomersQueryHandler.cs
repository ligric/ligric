using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ligric.Application.Configuration.Data;
using Ligric.Application.Configuration.Queries;

namespace Ligric.Application.Customers.GetCustomers
{
    public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, IEnumerable<CustomerDetailsDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetCustomersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<CustomerDetailsDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            //var connection = _sqlConnectionFactory.GetOpenConnection();
            //using var result = await connection.QueryMultipleAsync(sql);
            //return await result.ReadAsync<CustomerDetailsDto>();
            return null;
        }
    }
}