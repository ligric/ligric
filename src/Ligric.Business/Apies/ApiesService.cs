using Ligric.Business.Authorization;
using Ligric.Domain.Types.Api;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Ligric.Business.Apies
{
	public class ApiesService : IApiesService
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly List<ApiClientDto> _availableApies = new List<ApiClientDto>();

		public ApiesService(IAuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
		}

		public IReadOnlyCollection<ApiClientDto> AvailableApies => new ReadOnlyCollection<ApiClientDto>(_availableApies);

		//public event NotifyCollectionChangedEventHandler? ApiesChanged;

		public Task SaveApiAsync(ApiDto api, CancellationToken ct)
		{
			//_authorizationService.CurrentUser.
			throw new System.NotImplementedException();
		}

		public Task SetStateAsync(long id, StateEnum state, CancellationToken ct)
		{
			throw new System.NotImplementedException();
		}

		public Task SetStateAsync(IReadOnlyDictionary<long, ApiActivityStateFilter> multiChangesInfo, CancellationToken ct)
		{
			throw new System.NotImplementedException();
		}
	}
}
