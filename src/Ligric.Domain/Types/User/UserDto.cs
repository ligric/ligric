namespace Ligric.Domain.Types.User
{
    public class UserDto
    {
		public long? Id { get; }
        public string? UserName { get; }

        public UserDto(long? id, string? userName)
        {
			Id = id;
            UserName = userName;
        }
    }
}
