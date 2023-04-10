using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IUserApiRepository : IRepository<UserApiEntity>
	{
		IEnumerable<ApiClientResponseDto> GetAllowedApiInfoByUserId(long id);

		/// <summary>
		/// TEMPORARY
		/// </summary>
		/// <param name="userApiId"></param>
		IEnumerable<long> GetUserIdsThatDontHaveTheseApi(long userApiId);
	}
}
