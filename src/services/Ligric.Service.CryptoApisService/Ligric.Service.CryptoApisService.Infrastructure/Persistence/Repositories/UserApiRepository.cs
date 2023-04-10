using System;
using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;
using Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Database;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Repositories
{
	public class UserApiRepository : RepositoryBase<UserApiEntity>, IUserApiRepository
	{
        public UserApiRepository(DataProvider dataProvider)
            : base(dataProvider)
        {
        }

		public IEnumerable<ApiClientResponseDto> GetAllowedApiInfoByUserId(long id)
		{
			List<ApiClientResponseDto> apiClients = new List<ApiClientResponseDto>();
			var userApiesObjectList = DataProvider.CreateSqlQuery("EXEC [GetAllowedAPI] @userId = N'" + id + "'")?
				.List() ?? new List<object>();

			foreach (object[] item in userApiesObjectList)
			{
				long userApi = Convert.ToInt64(item[0]);
				string name = item[1] == null ? "Empty" : item[1].ToString();
				int permissions = Convert.ToInt32(item[2]);
				apiClients.Add(new ApiClientResponseDto(userApi, name, permissions));
			}

			return apiClients;
		}
	}
}
