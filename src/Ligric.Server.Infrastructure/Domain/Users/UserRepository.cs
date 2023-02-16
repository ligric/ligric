using Ligric.Server.Domain.Entities.Users;
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

        public UserEntity GetEntity(string? username)
        {
            var user = DataProvider.QueryOver<UserEntity>()
             .WhereRestrictionOn(x => x.UserName).IsInsensitiveLike(username, MatchMode.Exact)
             .SingleOrDefault();
            return user;
        }
    }
}
