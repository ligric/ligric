//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Grpc.Net.Client;
//using Ligric.Core.Types.Future;
//using static Ligric.Protobuf.Futures;
//using Utils;
//using Ligric.Business.Authorization;
//using System.Threading;
//using Ligric.Protobuf;
//using System.Linq;
//using Ligric.Business.Metadata;
//using Ligric.Business.Extensions;
//using Ligric.Business.Futures;

//namespace Ligric.Business.Clients.Futures
//{
//	public class OrdersService : IOrdersService
//	{
//		private int syncOrderChanged = 0;
//		private readonly Dictionary<long, FuturesOrderDto> _openOrders = new Dictionary<long, FuturesOrderDto>();
//		private CancellationTokenSource? _futuresSubscribeCalcellationToken;
//		private readonly IAuthorizationService _authorizationService;
//		private readonly IMetadataManager _metadataManager;
//		private readonly FuturesClient _futuresClient;

//		internal OrdersService(
//			FuturesClient futuresClient,
//			IMetadataManager metadataRepos,
//			IAuthorizationService authorizationService)
//		{
//			_metadataManager = metadataRepos;
//			_authorizationService = authorizationService;
//			_futuresClient = futuresClient;
//		}

//		public IReadOnlyDictionary<long, FuturesOrderDto> OpenOrders => new ReadOnlyDictionary<long, FuturesOrderDto>(_openOrders);

//		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OpenOrdersChanged;

//		public Task AttachStreamAsync(long userApiId)
//		{
//			if (_futuresSubscribeCalcellationToken != null
//				&& !_futuresSubscribeCalcellationToken.IsCancellationRequested)
//			{
//				return Task.CompletedTask;
//			}

//			var userId = _authorizationService.CurrentUser.Id ?? throw new NullReferenceException("[AttachStreamAsync] UserId is null");

//			_futuresSubscribeCalcellationToken = new CancellationTokenSource();
//			return StreamApiSubscribeCall(userId, userApiId, _futuresSubscribeCalcellationToken.Token);
//		}

//		public void DetachStream() => throw new NotImplementedException();
//		public void Dispose()
//		{

//		}

//		private Task StreamApiSubscribeCall(long userId, long userApiId, CancellationToken token)
//		{
//			var call = _futuresClient.OrdersSubscribe(
//				request: new FuturesSubscribeRequest { UserId = userId, UserApiId = userApiId },
//				headers: _metadataManager.CurrentMetadata,
//				cancellationToken: token);

//			return call.ResponseStream
//				.ToAsyncEnumerable()
//				.Finally(() => call.Dispose())
//				.ForEachAsync((api) =>
//				{
//					OnFuturesChanged(api);
//				}, token);
//		}

//		private void OnFuturesChanged(OrdersChanged api)
//		{
//			switch (api.Action)
//			{
//				case Rpc.Contracts.Action.Added:
//					var orderDto = api.Order.ToFuturesOrderDto();
//					_openOrders.SetAndRiseEvent(this, OpenOrdersChanged, api.Order.Id, orderDto, ref syncOrderChanged);
//					break;
//				case Rpc.Contracts.Action.Removed:
//					_openOrders.RemoveAndRiseEvent(this, OpenOrdersChanged, api.Order.Id, ref syncOrderChanged);
//					break;
//				case Rpc.Contracts.Action.Changed: goto case Rpc.Contracts.Action.Added;
//			}
//		}
//	}
//}
