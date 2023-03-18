using System.Threading.Tasks;

namespace Ligric.Service.AuthService.Infrastructure.Processing
{
	public interface IDomainEventsDispatcher
	{
		Task DispatchEventsAsync();
	}
}