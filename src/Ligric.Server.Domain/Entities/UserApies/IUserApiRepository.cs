using System.Collections.Generic;
using Ligric.Server.Domain.Entities.Apies;

namespace Ligric.Server.Domain.Entities.UserApies
{
	public interface IUserApiRepository
	{
		IEnumerable<UserApiEntity> GetEntitiesByUserId(long id);

		object Save(UserApiEntity entity);

		void Delete(long id);

		void Delete(UserApiEntity entity);
	}
}
