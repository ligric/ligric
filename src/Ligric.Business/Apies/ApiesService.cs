using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Ligric.Business.Authorization;
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

		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;
		private UserApisClient _client;
		private CancellationTokenSource? _apiPiplineSubscriveCancellationToken;
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

		public event NotifyCollectionChangedEventHandler? ApiesChanged;

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
			if (_apiPiplineSubscriveCancellationToken is not null && !_apiPiplineSubscriveCancellationToken.IsCancellationRequested)
				return Task.CompletedTask;

			var call = _client.ApisSubscribe(new Empty());
			_apiPiplineSubscriveCancellationToken = new CancellationTokenSource();

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(() => call.Dispose())
				.ForEachAsync((x) =>
				{
					ApiClientDto apiClient = new(x.Api.Id, x.Api.Name, x.Api.Permissions);
					switch(x.Action)
					{
						case Protos.Action.Added:
							_availableApies.Add(apiClient);
							ApiesChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, apiClient));
							break;
					}

				}, _apiPiplineSubscriveCancellationToken.Token);
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
