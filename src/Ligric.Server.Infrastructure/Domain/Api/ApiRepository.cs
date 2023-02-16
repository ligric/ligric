using Ligric.Server.Data.Base;
using Ligric.Server.Domain.Entities.Apies;
using Ligric.Server.Domain.Entities.Apis;

namespace Ligric.Infrastructure.Domain.Api
{
	public class ApiRepository : RepositoryBase<ApiEntity>, IApiRepository
	{
		public ApiRepository(DataProvider dataProvider)
			: base(dataProvider)
		{
		}
	}
}
