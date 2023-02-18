using Grpc.Net.Client;
using Ligric.Business.Authorization;
using Ligric.Business.Metadata;
using Ligric.Domain.Types.Api;
using Ligric.Protos;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using static Ligric.Protos.UserApis;

namespace Ligric.Business.Apies
{
	public class ApiesService : IApiesService
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;
		private UserApisClient _client;

		private readonly List<ApiClientDto> _availableApies = new List<ApiClientDto>();

		public ApiesService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos,
			IAuthorizationService authorizationService)
		{
			_metadataManager = metadataRepos;
			_authorizationService = authorizationService;

			_client = new UserApisClient(grpcChannel);
		}

		public IReadOnlyCollection<ApiClientDto> AvailableApies => new ReadOnlyCollection<ApiClientDto>(_availableApies);

		//public event NotifyCollectionChangedEventHandler? ApiesChanged;

		public async Task SaveApiAsync(ApiDto api, CancellationToken ct)
		{
			var userId = _authorizationService.CurrentUser.Id ?? throw new System.ArgumentNullException($"SaveApiAsync : UserId is null");
			var response = await _client.SaveAsync(new SaveApiRequest
			{
				Name = api.Name,
				OwnerId = userId,
				PrivateKey = api.PrivateKey,
				PublicKey = api.PublicKey,
			}, headers: _metadataManager.CurrentMetadata, cancellationToken: ct);

			if (response is null)
			{
				throw new System.NotImplementedException();
			}

			var newApi = new ApiClientDto(response.ApiId, api.Name, 31);
			_availableApies.Add(newApi);

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
