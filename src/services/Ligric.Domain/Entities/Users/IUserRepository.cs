using System.Collections.Generic;
using Ligric.Core.Types.User;

namespace Ligric.Domain.Entities.Users
{
    public interface IUserRepository
    {
		UserEntity GetEntityById(long id);

		UserEntity GetEntity(string username);

		bool UserNameIsExists(string username);

		IEnumerable<long> GetUserIdsThatDontHaveTheseApi(long userApiId);

		object Save(UserEntity entity);
    }
}
