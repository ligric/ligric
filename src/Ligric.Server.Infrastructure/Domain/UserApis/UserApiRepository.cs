using Ligric.Server.Data.Base;
using Ligric.Server.Domain.Entities.UserApies;
using System.Collections.Generic;

namespace Ligric.Infrastructure.Domain.Users
{
	public class UserApiRepository : RepositoryBase<UserApiEntity>, IUserApiRepository
	{
        public UserApiRepository(DataProvider dataProvider)
            : base(dataProvider)
        {
        }

		public IEnumerable<UserApiEntity> GetEntitiesByUserId(long id)
		{
			var userApies = DataProvider.CreateSqlQuery("EXEC [GetAllowedAPI] @userId = N'" + id + "'")?
				.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(UserApiEntity)))
				.List<UserApiEntity>();

			return userApies ?? new List<UserApiEntity>();
		}
    }
}
