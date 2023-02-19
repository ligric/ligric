using System;
using Newtonsoft.Json;

namespace Microservices.Library.EventBus.Events
{
    /// <summary>
    /// The integration test class describes an event that is transmitted between micro-services.
    /// </summary>
    public class IntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        #region CONSTRUCTORS

        /// <summary>
        /// Creates a new instance of an integration event
        /// </summary>
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new instance of an integration event
        /// </summary>
        /// <param name="id">The aggregate id</param>
        /// <param name="createDate">The creation datetime</param>
        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        #endregion

    }
}
