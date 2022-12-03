using Ligric.Common.Dto;

namespace Ligric.Server.Domain.Entities.Users
{
    public interface IUserRepository
    {
        UserDto Get(long? id);

        bool IsUserNameUnique(string username);

        object Save(UserEntity entity);

        void Delete(long id);
    }
}