﻿using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;
using Ligric.Business.Authorization;
using Ligric.Protobuf;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using Ligric.Core.Types;
using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients.Futures
{
	public class OrdersService : IOrdersService
	{
		private int syncOrderChanged = 0;
		private readonly Dictionary<long, ExchangedEntity<FuturesOrderDto>> _openOrders = new Dictionary<long, ExchangedEntity<FuturesOrderDto>>();
		private CancellationTokenSource? _futuresSubscribeCalcellationToken;
		private readonly IAuthorizationService _authorizationService;
		private readonly IMetadataManager _metadataManager;
		private readonly FuturesClient _futuresClient;

		internal OrdersService(
			FuturesClient futuresClient,
			IMetadataManager metadataRepos,
			IAuthorizationService authorizationService)
		{
			_metadataManager = metadataRepos;
			_authorizationService = authorizationService;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<long, ExchangedEntity<FuturesOrderDto>> OpenOrders => new ReadOnlyDictionary<long, ExchangedEntity<FuturesOrderDto>>(_openOrders);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, ExchangedEntity<FuturesOrderDto>>>? OpenOrdersChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_futuresSubscribeCalcellationToken != null
				&& !_futuresSubscribeCalcellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _authorizationService.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

			_futuresSubscribeCalcellationToken = new CancellationTokenSource();
			return StreamApiSubscribeCall(userId, userApiId, _futuresSubscribeCalcellationToken.Token);
		}

		public void DetachStream()
		{
			_futuresSubscribeCalcellationToken?.Cancel();
			_futuresSubscribeCalcellationToken?.Dispose();
		}

		public void Dispose()
		{

		}

		private Task StreamApiSubscribeCall(long userId, long userApiId, CancellationToken token)
		{
			var call = _futuresClient.OrdersSubscribe(
				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
				headers: _metadataManager.CurrentMetadata,
				cancellationToken: token);

			return call.ResponseStream
				.ToAsyncEnumerable()
				.Finally(call.Dispose)
				.ForEachAsync(OnFuturesChanged, token);
		}

		private void OnFuturesChanged(OrdersChanged api)
		{
			switch (api.Action)
			{
				case Protobuf.Action.Added:
					var exchangedOrderDto = new ExchangedEntity<FuturesOrderDto>(
						Guid.Parse(api.ExchangeId),
						api.Order.ToFuturesOrderDto());

					_openOrders.SetAndRiseEvent(this, OpenOrdersChanged, api.Order.Id, exchangedOrderDto, ref syncOrderChanged);
					break;
				case Protobuf.Action.Removed:
					_openOrders.RemoveAndRiseEvent(this, OpenOrdersChanged, api.Order.Id, ref syncOrderChanged);
					break;
				case Protobuf.Action.Changed: goto case Protobuf.Action.Added;
			}
		}
	}
}
