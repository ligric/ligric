using System.Threading.Tasks;
using Ligric.Contracts.Models;
using Ligric.Domain.Models;

namespace Ligric.Application.Bus
{
    /// <summary>
    /// It is used as a message broker to enable microservices communicating each other
    /// </summary>
    public interface IMessageBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent: IntegrationEvent;
    }
}
