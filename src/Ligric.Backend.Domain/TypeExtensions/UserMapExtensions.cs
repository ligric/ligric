using Ligric.Types.User;
using Ligric.Backend.Domain.Entities.Users;

namespace Ligric.Backend.Domain.TypeExtensions
{
    public static class UserMapExtensions
    {
        public static UserDto ToUserDto(this UserEntity user)
        {
            return new UserDto(user.Id, user.UserName);
        }
    }
}
