using System.Collections.Generic;
using Ligric.Service.AuthService.Domain.Entities;
using System.Threading.Tasks;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IOutboxMessageRepository : IRepositoryBase<OutboxMessage>
	{
		Task AddAsync(List<UserEntity> entity);
	}
}
