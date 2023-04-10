using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IApiRepository
	{
		ApiEntity GetEntityById(long id);

		ApiEntity GetEntityByUserApiId(long id);

		object Save(ApiEntity entity);

		void Delete(long id);

		void Delete(ApiEntity entity);
	}
}
