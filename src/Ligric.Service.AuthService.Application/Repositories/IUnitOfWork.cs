using System;
using System.Threading.Tasks;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IUnitOfWork : IDisposable
	{

		IOutboxMessageRepository OutboxMessageRepository { get; }

		Task SaveChangesAsync();
	}
}
