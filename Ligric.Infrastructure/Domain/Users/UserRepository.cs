using System;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Common.Dto;

namespace Ligric.Infrastructure.Domain.Users
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public UserDto Get(long? id)
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
    }
}