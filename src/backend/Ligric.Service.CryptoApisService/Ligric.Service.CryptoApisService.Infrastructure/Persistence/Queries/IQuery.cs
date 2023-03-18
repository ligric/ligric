using MediatR;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Queries
{
	public interface IQuery<out TResult> : IRequest<TResult>
	{

	}
}