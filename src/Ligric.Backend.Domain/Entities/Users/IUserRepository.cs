using System.Collections.Generic;
using Ligric.Types.User;

namespace Ligric.Backend.Domain.Entities.Users
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
