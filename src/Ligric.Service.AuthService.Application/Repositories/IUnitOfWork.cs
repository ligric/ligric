using System;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IOutboxMessageRepository OutboxMessageRepository { get; }
	}
}
