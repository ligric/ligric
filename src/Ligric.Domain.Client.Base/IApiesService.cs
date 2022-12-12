using Common.Enums;
using Ligric.Common.Types.Api;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Ligric.Domain.Client.Base
{
    public interface IApiesService
    {
        IReadOnlyCollection<ApiClientDto> AvailableApies { get; }

        event NotifyCollectionChangedEventHandler? ApiesChanged;

        /// <summary>
        /// Saving api local or remote and available only for current user.
        /// </summary>
        /// <param name="api">Api info</param>
        Task SaveApiAsync(ApiDto api);

        /// <summary>
        /// Set state for everyuser.
        /// </summary>
        /// <param name="id">Api id.</param>
        /// <param name="state">New activity state.</param>
        /// <remarks>Wors if user has permissions or users is owner.</remarks>
        Task SetStateAsync(long id, StateEnum state);

        /// <summary>
        /// Multiselection apies and users
        /// </summary>
        /// <param name="multiChangesInfo">Selection info</param>
        /// <remarks>Wors if user has permissions or users is owner.</remarks>
        Task SetStateAsync(IReadOnlyDictionary<long, ApiActivityStateFilter> multiChangesInfo);
    }
}
