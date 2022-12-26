using Ligric.Domain.Types.User;
using Ligric.Server.Domain.Entities.Users;

namespace Ligric.Server.Domain.TypeExtensions
{
    public static class UserMapExtensions
    {
        public static UserDto ToUserDto(this UserEntity user)
        {
            return new UserDto(user.Id, user.UserName);
        }
    }
}
