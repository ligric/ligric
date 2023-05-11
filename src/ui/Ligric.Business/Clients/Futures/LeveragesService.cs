﻿using System.Collections.ObjectModel;
using Ligric.Business.Authorization;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using Ligric.Business.Metadata;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;
using Utils;
using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients.Futures
{
	public class LeveragesService : ILeveragesService
	{
		private int syncLeveragesChanged = 0;

		private readonly Dictionary<Guid, LeverageDto> _leverages = new Dictionary<Guid, LeverageDto>();
		private CancellationTokenSource? _futuresSubscribeCalcellationToken;
		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		public LeveragesService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			IAuthorizationService authorizationService)
		{
			_metadataManager = metadataRepos;
			_authorizationService = authorizationService;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<Guid, LeverageDto> Leverages => new ReadOnlyDictionary<Guid, LeverageDto>(_leverages);

		public event EventHandler<NotifyDictionaryChangedEventArgs<Guid, LeverageDto>>? LeveragesChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_futuresSubscribeCalcellationToken != null
			&& !_futuresSubscribeCalcellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}
			var userId = _authorizationService.CurrentUser.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");
			_futuresSubscribeCalcellationToken = new CancellationTokenSource();
			return StreamApiSubscribeCall(userId, userApiId, _futuresSubscribeCalcellationToken.Token);
		}

		public void Dispose()
		{

		}

		private Task StreamApiSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.LeverageSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(call.Dispose)
				.ForEachAsync(OnLeveragesChanged, token);
		}

		private void OnLeveragesChanged(LeverageChanged changes)
		{
			switch (changes.Action)
			{
				case Protobuf.Action.Added:
					var leverageDto = changes.Leverage.ToFuturesLeverageDto();
					_leverages.AddAndRiseEvent(this, LeveragesChanged, Guid.Parse(changes.ExchangeId), leverageDto, ref syncLeveragesChanged);
					break;
			}
		}
	}
}