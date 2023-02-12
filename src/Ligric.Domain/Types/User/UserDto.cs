namespace Ligric.Domain.Types.User
{
    public class UserDto
    {
        public string? UserName { get; }

        public UserDto(string? userName)
        {
            UserName = userName;
        }
    }
}
