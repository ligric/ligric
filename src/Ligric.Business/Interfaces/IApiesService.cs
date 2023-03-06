using Ligric.Domain.Types.Api;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Ligric.Business.Apies
{
	public interface IApiesService : IDisposable
	{
		IReadOnlyCollection<ApiClientDto> AvailableApies { get; }
		
		event NotifyCollectionChangedEventHandler? ApiesChanged;

		/// <summary>
		/// Saving api local or remote and available only for current user.
		/// </summary>
		/// <param name="api">Api info</param>
		/// <returns>Api id</returns>
		Task<long> SaveApiAsync(string name, string privateKey, string publicKey, CancellationToken ct);

		Task ShareApiAsync(ApiClientDto api, CancellationToken ct);

		/// <summary>
		/// Set state for everyuser.
		/// </summary>
		/// <param name="id">Api id.</param>
		/// <param name="state">New activity state.</param>
		/// <remarks>Wors if user has permissions or users is owner.</remarks>
		Task SetStateAsync(long id, StateEnum state, CancellationToken ct);

		/// <summary>
		/// Multiselection apies and users
		/// </summary>
		/// <param name="multiChangesInfo">Selection info</param>
		/// <remarks>Wors if user has permissions or users is owner.</remarks>
		Task SetStateAsync(IReadOnlyDictionary<long, ApiActivityStateFilter> multiChangesInfo, CancellationToken ct);

		Task ApiPiplineSubscribeAsync();

		void ApiPiplineUnsubscribe();
	}
}
