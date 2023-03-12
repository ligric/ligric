using NHibernate.Criterion;
using System.Collections.Generic;
using System;
using Ligric.Service.AuthService.Domain.Entities;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Infrastructure.Database;

namespace Ligric.Service.AuthService.Infrastructure.Persistence.Repositories
{
	public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(DataProvider dataProvider)
            : base(dataProvider)
        {
        }

        public UserEntity GetEntityByUserName(string? username)
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

		public bool UserNameIsExists(string username) => throw new NotImplementedException();
	}
}
