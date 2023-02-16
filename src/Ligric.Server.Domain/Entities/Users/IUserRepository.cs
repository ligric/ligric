using Ligric.Domain.Types.User;

namespace Ligric.Server.Domain.Entities.Users
{
    public interface IUserRepository
    {
		UserEntity GetEntityById(long id);

		UserEntity GetEntity(string username);

		bool UserNameIsExists(string username);

		object Save(UserEntity entity);
    }
}
