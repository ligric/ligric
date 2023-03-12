using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Infrastructure.Database;

namespace Ligric.Service.AuthService.Infrastructure.Persistence.Repositories
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
