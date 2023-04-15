using Ligric.Service.CryptoApisService.Application.Repositories;
using Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Database;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private IOutboxMessageRepository? _outboxMessageRepository;
		private readonly DataProvider _dataProvider;

		public UnitOfWork(DataProvider dataProvider)
		{
			_dataProvider = dataProvider;
		}

		public IOutboxMessageRepository OutboxMessageRepository
		{
			get
			{
				if (_outboxMessageRepository != null)
					return _outboxMessageRepository;
				return _outboxMessageRepository = new OutBoxRepository(_dataProvider);
			}
		}

		public void Dispose()
		{
			_dataProvider.Session.Dispose();
		}
	}
}
