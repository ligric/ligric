using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IOutboxMessageRepository : IRepository<OutboxMessage>
	{
		void Save(List<UserApiEntity> entity);
	}
}
