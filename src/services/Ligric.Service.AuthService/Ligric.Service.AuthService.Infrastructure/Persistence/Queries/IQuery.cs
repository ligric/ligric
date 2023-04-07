using MediatR;

namespace Ligric.Service.AuthService.Infrastructure.Persistence.Queries
{
	public interface IQuery<out TResult> : IRequest<TResult>
	{

	}
}