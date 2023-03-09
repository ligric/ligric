using System.Collections.Generic;
using Ligric.Types.Api;
using Ligric.Backend.Domain.Entities.Apies;

namespace Ligric.Backend.Domain.Entities.UserApies
{
	public interface IUserApiRepository
	{
		IEnumerable<ApiClientDto> GetAllowedApiInfoByUserId(long id);

		UserApiEntity GetEntityById(long id);

		object Save(UserApiEntity entity);

		void Delete(long id);

		void Delete(UserApiEntity entity);
	}
}
