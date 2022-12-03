namespace Ligric.Common.Dto
{
    public class UserDto
    {
        public string Id { get; }

        public string UserName { get; }

        public UserDto(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
