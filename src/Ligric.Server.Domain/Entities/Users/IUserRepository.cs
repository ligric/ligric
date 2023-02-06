using Ligric.Domain.Types.User;

namespace Ligric.Server.Domain.Entities.Users
{
    public interface IUserRepository
    {
        UserDto Get(long? id);

        UserDto Get(string? username);

        UserEntity GetEntity(string? username);

        bool IsUserNameUnique(string username);

        object Save(UserEntity entity);

        void Delete(long id);
    }
}