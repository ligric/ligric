using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Database;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Repositories
{
	public class OutBoxRepository : RepositoryBase<OutboxMessage>, IOutboxMessageRepository
	{
		public OutBoxRepository(DataProvider dataProvider)
			: base(dataProvider)
		{

		}

		public void Save(List<UserApiEntity> entity)
		{
			var messages = OutboxMessage.ToManyMessages(entity);

			foreach (var message in messages)
			{
				this.Save(message);
			}
		}
	}
}
