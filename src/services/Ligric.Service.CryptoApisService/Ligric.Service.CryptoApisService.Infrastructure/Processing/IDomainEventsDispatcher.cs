using System.Threading.Tasks;

namespace Ligric.Service.CryptoApisService.Infrastructure.Processing
{
	public interface IDomainEventsDispatcher
	{
		Task DispatchEventsAsync();
	}
}