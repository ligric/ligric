using System;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IOutboxMessageRepository OutboxMessageRepository { get; }
	}
}
