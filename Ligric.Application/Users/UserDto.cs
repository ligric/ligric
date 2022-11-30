using System;

namespace Ligric.Application.Users
{
    public class UserDto
    {
        public Guid Id { get; }

        public string Login { get; }

        public UserDto(Guid id, string login)
        {
            Id = id;
            Login = login;
        }
    }
}