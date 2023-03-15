using MediatR;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Queries
{
	public interface IQueryHandler<in TQuery, TResult> :
		IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
	{

	}
}