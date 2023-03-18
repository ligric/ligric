﻿using Ligric.Core.Ligric.Core.Types.Api;
using System.Collections.Generic;
using System;
using Ligric.Data.Base;
using Ligric.Domain.Entities.Apies;
using Ligric.Domain.Entities.Apis;
using NHibernate.Transform;
using System.Linq;

namespace Ligric.Infrastructure.Domain.Api
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