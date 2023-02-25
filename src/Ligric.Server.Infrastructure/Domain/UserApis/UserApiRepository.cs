using Ligric.Domain.Types.Api;
using Ligric.Server.Data.Base;
using Ligric.Server.Domain.Entities.UserApies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ligric.Infrastructure.Domain.Users
{
	public class UserApiRepository : RepositoryBase<UserApiEntity>, IUserApiRepository
	{
        public UserApiRepository(DataProvider dataProvider)
            : base(dataProvider)
        {
        }

		public IEnumerable<ApiClientDto> GetAllowedApiInfoByUserId(long id)
		{
			List<ApiClientDto> apiClients = new List<ApiClientDto>();
			var userApiesObjectList = DataProvider.CreateSqlQuery("EXEC [GetAllowedAPI] @userId = N'" + id + "'")?
				.List() ?? new List<object>();

			foreach (object[] item in userApiesObjectList)
			{
				long userApi = Convert.ToInt64(item[0]);
				string name = item[1] == null ? "Empty" : item[1].ToString();
				int permissions = Convert.ToInt32(item[2]);
				apiClients.Add(new ApiClientDto(userApi, name, permissions));
			}

			return apiClients;
		}
    }
}
