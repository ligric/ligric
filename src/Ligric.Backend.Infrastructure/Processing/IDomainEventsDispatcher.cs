using System.Threading.Tasks;

namespace Ligric.Backend.Infrastructure.Processing
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}