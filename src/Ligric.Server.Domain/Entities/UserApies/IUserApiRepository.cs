using System.Collections.Generic;
using Ligric.Domain.Types.Api;
using Ligric.Server.Domain.Entities.Apies;

namespace Ligric.Server.Domain.Entities.UserApies
{
	public interface IUserApiRepository
	{
		IEnumerable<ApiClientDto> GetAllowedApiInfoByUserId(long id);

		object Save(UserApiEntity entity);

		void Delete(long id);

		void Delete(UserApiEntity entity);
	}
}
