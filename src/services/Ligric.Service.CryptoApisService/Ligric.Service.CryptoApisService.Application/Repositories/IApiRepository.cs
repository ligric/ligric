using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IApiRepository : IRepository<ApiEntity>
	{
		ApiEntity GetEntityByUserApiId(long id);
	}
}
