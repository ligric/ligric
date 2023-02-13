using System;
using Ligric.Server.Domain.Entities.Users;
using Ligric.Server.Data.Base;
using NHibernate.Criterion;
using Ligric.Domain.Types.User;

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

        public override void Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}
