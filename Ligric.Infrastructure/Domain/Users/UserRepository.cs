using System;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Common.Types;

namespace Ligric.Infrastructure.Domain.Users
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
        }

        public UserDto Get(long? id)
        {
            throw new NotImplementedException();
        }

        public UserDto Get(string? username)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetEntity(string? username)
        {


            throw new NotImplementedException();
        }

        public bool IsUserNameUnique(string username)
        {
            throw new NotImplementedException();
        }

        public object Save(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}