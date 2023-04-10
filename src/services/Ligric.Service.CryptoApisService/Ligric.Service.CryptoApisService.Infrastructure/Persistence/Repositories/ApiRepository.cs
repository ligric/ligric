using System;
using NHibernate.Transform;
using System.Linq;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Database;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Repositories
{
	public class ApiRepository : RepositoryBase<ApiEntity>, IApiRepository
	{
		public ApiRepository(DataProvider dataProvider)
			: base(dataProvider)
		{
		}

		public ApiEntity GetEntityByUserApiId(long id)
		{
			var sqlQuery = DataProvider.CreateSqlQuery("EXEC [GetApiByUserApiId] @userApiId = N'" + id + "'");
			if (sqlQuery == null) throw new ArgumentException("[GetApiByUserApiId] wrong request.");

			var api = sqlQuery.SetResultTransformer(Transformers.AliasToBean(typeof(ApiEntity))).List<ApiEntity>().Single();

			return api;
		}
	}
}
