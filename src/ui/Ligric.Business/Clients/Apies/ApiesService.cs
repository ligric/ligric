using Grpc.Net.Client;
using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Extensions;
using Ligric.Business.Metadata;
using System.Collections.Specialized;
using Utils;
using Ligric.Core.Ligric.Core.Types.Api;
using static Ligric.Protobuf.UserApis;
using Ligric.Protobuf;
using Ligric.Business.Interfaces;

namespace Ligric.Business.Clients.Apies
{
	public class ApiesService : IApiesService
	{
		private bool disposed = false;
		private UserApisClient _client;
		private CancellationTokenSource? _apiPiplineSubscriveCancellationToken;

		private readonly HashSet<ApiClientDto> _availableApies = new HashSet<ApiClientDto>();
		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;

		internal ApiesService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_client = new UserApisClient(grpcChannel);
			_currentUser = currentUser;
		}

		public IReadOnlyCollection<ApiClientDto> AvailableApies => _availableApies;

		public event NotifyCollectionChangedEventHandler? ApiesChanged;

		public async Task<long> SaveApiAsync(string name, string privateKey, string publicKey, CancellationToken ct)
		{
			var userId = _currentUser.CurrentUser?.Id ?? throw new ArgumentNullException($"SaveApiAsync : UserId is null");
			var response = await _client.SaveAsync(new SaveApiRequest
			{
				Name = name,
				OwnerId = userId,
				PrivateKey = privateKey,
				PublicKey = publicKey,
			}, headers: _metadataManager.CurrentMetadata, cancellationToken: ct);

			var apiId = response.ApiId;

			var newApi = new ApiClientDto(apiId, name, 31);
			_availableApies.AddAndRiseEvent(this, ApiesChanged, newApi);
			return apiId;
		}

		public async Task ShareApiAsync(ApiClientDto api, CancellationToken ct)
		{
			var userId = _currentUser.CurrentUser?.Id ?? throw new ArgumentNullException($"SaveApiAsync : UserId is null");
			var response = await _client.ShareAsync(new ShareApiRequest
			{
				OwnerId = userId,
				UserApiId = api.UserApiId ?? throw new ArgumentNullException($"UserApiId : UserApiId is null"),
				Permissions = 1,
			}, headers: _metadataManager.CurrentMetadata, cancellationToken: ct);
		}

		public Task SetStateAsync(long id, StateEnum state, CancellationToken ct)
		{
			throw new NotImplementedException();
		}

		public Task SetStateAsync(IReadOnlyDictionary<long, ApiActivityStateFilter> multiChangesInfo, CancellationToken ct)
		{
			throw new NotImplementedException();
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
			var currentUserId = _currentUser.CurrentUser?.Id;
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
			var apiClient = changedInfo.Api.ToApiClientDto();
			switch (changedInfo.Action)
			{
				case Protobuf.Action.Added:
					_availableApies.AddAndRiseEvent(this, ApiesChanged, apiClient);
					break;
				case Protobuf.Action.Removed:
					_availableApies.RemoveAndRiseEvent(this, ApiesChanged, apiClient);
					break;
				case Protobuf.Action.Changed:
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
