using System.Collections.Generic;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Domain.Entities.Apies;

namespace Ligric.Domain.Entities.UserApies
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
