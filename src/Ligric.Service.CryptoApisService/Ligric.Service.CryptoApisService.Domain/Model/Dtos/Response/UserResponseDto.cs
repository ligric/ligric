namespace Ligric.Core.Types.User
{
    public class UserResponseDto
    {
		public long Id { get; }
        public string UserName { get; }

        public UserResponseDto(long id, string userName)
        {
			Id = id;
            UserName = userName;
        }
    }
}
