using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Ligric.Business.Authorization;
using Ligric.Business.Extensions;
using Ligric.Business.Metadata;
using Ligric.Domain.Types.Api;
using Ligric.Protos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using static Ligric.Protos.UserApis;

namespace Ligric.Business.Apies
{
	public class ApiesService : IApiesService
	{
		private bool disposed = false;
		private UserApisClient _client;
		private CancellationTokenSource? _apiPiplineSubscriveCancellationToken;

		private readonly HashSet<ApiClientDto> _availableApies = new HashSet<ApiClientDto>();
		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;

		public ApiesService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos,
			IAuthorizationService authorizationService)
		{
			_metadataManager = metadataRepos;
			_client = new UserApisClient(grpcChannel);
			_authorizationService = authorizationService;
		}

		public IReadOnlyCollection<ApiClientDto> AvailableApies => _availableApies;

		public event NotifyCollectionChangedEventHandler? ApiesChanged;

		public async Task<long> SaveApiAsync(ApiDto api, CancellationToken ct)
		{
			var userId = _authorizationService.CurrentUser.Id ?? throw new System.ArgumentNullException($"SaveApiAsync : UserId is null");
			var response = await _client.SaveAsync(new SaveApiRequest
			{
				Name = api.Name,
				OwnerId = userId,
				PrivateKey = api.PrivateKey,
				PublicKey = api.PublicKey,
			}, headers: _metadataManager.CurrentMetadata, cancellationToken: ct);

			var apiId = response.ApiId;

			var newApi = new ApiClientDto(apiId, api.Name, 31);
			_availableApies.AddAndRiseEvent(this, newApi, ApiesChanged);
			return apiId;
		}

		public Task SetStateAsync(long id, StateEnum state, CancellationToken ct)
		{
			throw new System.NotImplementedException();
		}

		public Task SetStateAsync(IReadOnlyDictionary<long, ApiActivityStateFilter> multiChangesInfo, CancellationToken ct)
		{
			throw new System.NotImplementedException();
		}

		public Task ApiPiplineSubscribeAsync()
		{
			if (_apiPiplineSubscriveCancellationToken != null
				&& !_apiPiplineSubscriveCancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			_apiPiplineSubscriveCancellationToken = new CancellationTokenSource();
			return StreamApiSubscribeCall(_apiPiplineSubscriveCancellationToken.Token);
		}

		public void ApiPiplineUnsubscribe()
		{
			_apiPiplineSubscriveCancellationToken?.Cancel();
			_apiPiplineSubscriveCancellationToken?.Dispose();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private Task StreamApiSubscribeCall(CancellationToken token)
		{
			var currentUserId = _authorizationService.CurrentUser.Id;
			var call = _client.ApisSubscribe(
				request: new ApiSubscribeRequest { UserId = currentUserId ?? -1 },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(() => call.Dispose())
				.ForEachAsync((api) =>
				{
					OnServerApisChanged(api);
				}, token);
		}

		private void OnServerApisChanged(ApisChanged changedInfo)
		{
			ApiClientDto apiClient = changedInfo.Api.ToApiClientDto();
			switch (changedInfo.Action)
			{
				case Protos.Action.Added:
					_availableApies.AddAndRiseEvent(this, apiClient, ApiesChanged);
					break;
				case Protos.Action.Removed:
					_availableApies.RemoveAndRiseEvent(this, apiClient, ApiesChanged);
					break;
				case Protos.Action.Changed:
					var oldApi = _availableApies.First(x => x.UserApiId == apiClient.UserApiId);
					_availableApies.UpdateAndRiseEvent(this, apiClient, oldApi, ApiesChanged);
					break;
			}
		}

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				ApiPiplineUnsubscribe();
			}

			disposed = true;
		}
	}
}
