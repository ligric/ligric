using System.Threading.Tasks;

namespace Ligric.Server.Infrastructure.Processing
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}