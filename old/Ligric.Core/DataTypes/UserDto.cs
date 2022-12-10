using System;
using Utils.DtoTypes;

namespace DevPace.Core.DataTypes
{
    public class UserDto : GuidDto
    {
        public string Login { get; }

        public string Email { get; }

        public UserDto(Guid guid, string login, string email)
            : base(guid)
        {
            Login = login ?? throw new ArgumentNullException("Login cannot be null.");
            Email = email ?? throw new ArgumentNullException("Email cannot be null.");
        }

        protected override bool EqualsCore(GuidDto dto)
        {
            return dto is UserDto other &&
                Equals(other.Login, Login) &&
                Equals(other.Email, Email);
        }
    }
}
