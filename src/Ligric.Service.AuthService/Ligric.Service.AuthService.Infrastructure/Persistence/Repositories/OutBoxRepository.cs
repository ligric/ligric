using System.Collections.Generic;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Domain.Entities;
using Ligric.Service.AuthService.Infrastructure.Nhibernate.Database;

namespace Ligric.Service.AuthService.Infrastructure.Persistence.Repositories
{
	public class OutBoxRepository : RepositoryBase<OutboxMessage>, IOutboxMessageRepository
	{
		public OutBoxRepository(DataProvider dataProvider)
			: base(dataProvider)
		{

		}

		public void Save(List<UserEntity> entity)
		{
			var messages = OutboxMessage.ToManyMessages(entity);

			foreach (var message in messages)
			{
				this.Save(message);
			}
		}
	}
}
