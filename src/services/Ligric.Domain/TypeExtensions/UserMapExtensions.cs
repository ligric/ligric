using Ligric.Core.Types.User;
using Ligric.Domain.Entities.Users;

namespace Ligric.Domain.TypeExtensions
{
    public static class UserMapExtensions
    {
        public static UserDto ToUserDto(this UserEntity user)
        {
            return new UserDto(user.Id, user.UserName);
        }
    }
}
