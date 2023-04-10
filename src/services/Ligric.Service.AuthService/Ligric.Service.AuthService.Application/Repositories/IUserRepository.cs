using System.Collections.Generic;
using Ligric.Service.AuthService.Domain.Entities;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IUserRepository : IRepository<UserEntity>
	{
		UserEntity GetEntityByUserName(string username);

		bool UserNameIsExists(string username);

		IEnumerable<long> GetUserIdsThatDontHaveTheseApi(long userApiId);
	}
}
