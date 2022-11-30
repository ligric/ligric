using System;
using Ligric.Domain.Users.Rules;
using Ligric.Domain.SeedWork;

namespace Ligric.Domain.Users
{
    public class User : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        private User()
        {
        }

        private User(Guid id, string login, string password)
        {
            Id = new UserId(id);
            Login = login;
            Password = password;
        }

        public static User CreateRegistered(
            string login,
            string password,
            IUniqueIdGenerator idGenerator)
        {
            Guid id = idGenerator.GetUniqueId();

            return new User(id, login, password);
        }
    }
}