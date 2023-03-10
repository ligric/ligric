using System.Collections.Generic;
using Ligric.Service.AuthService.Domain.Entities;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IUserRepository<TSQLQuery> : IRepository<UserEntity, TSQLQuery>
	{
		UserEntity GetEntityByUsername(string username);

		bool UserNameIsExists(string username);

		IEnumerable<long> GetUserIdsThatDontHaveTheseApi(long userApiId);
	}
}
