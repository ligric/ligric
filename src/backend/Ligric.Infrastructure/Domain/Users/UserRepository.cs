using Ligric.Domain.Entities.Users;
using Ligric.Data.Base;
using NHibernate.Criterion;
using Ligric.Core.Ligric.Core.Types.Api;
using System.Collections.Generic;
using System;

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

		public IEnumerable<long> GetUserIdsThatDontHaveTheseApi(long userApiId)
		{
			List<long> userIds = new List<long>();
			var userIdsObjectList = DataProvider.CreateSqlQuery("EXEC [GetUserIdsThatDontHaveTheseApi] @userApiId = N'" + userApiId + "'")?
				.List() ?? new List<object>();

			foreach (object item in userIdsObjectList)
			{
				long userId = Convert.ToInt64(item);
				userIds.Add(userId);
			}

			return userIds;
		}

		public bool UserNameIsExists(string username) => throw new System.NotImplementedException();
	}
}
