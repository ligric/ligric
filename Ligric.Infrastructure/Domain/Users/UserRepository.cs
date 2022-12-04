using System;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Common.Types;
using Ligric.Server.Data.Base;
using NHibernate.Criterion;

namespace Ligric.Infrastructure.Domain.Users
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(DataProvider dataProvider)
            : base(dataProvider)
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
            var user = DataProvider.QueryOver<UserEntity>()
             .WhereRestrictionOn(x => x.UserName).IsInsensitiveLike(username, MatchMode.Exact)
             .SingleOrDefault();

            return user;
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