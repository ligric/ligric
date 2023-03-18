using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IUserApiRepository
	{
		IEnumerable<ApiClientResponseDto> GetAllowedApiInfoByUserId(long id);

		UserApiEntity GetEntityById(long id);

		object Save(UserApiEntity entity);

		void Delete(long id);

		void Delete(UserApiEntity entity);
	}
}
