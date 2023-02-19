using System.Threading.Tasks;

namespace Microservices.Library.EventBus.Abstractions
{
    /// <summary>
    /// Contracts a dynamic integration event handler
    /// </summary>
    public interface IDynamicIntegrationEventHandler
    {
        /// <summary>
        /// Public async function that handles a dynamic event
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        Task Handle(dynamic eventData);
    }
}
