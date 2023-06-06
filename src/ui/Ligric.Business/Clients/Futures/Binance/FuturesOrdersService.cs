using System.Collections.ObjectModel;
using Ligric.Core.Types.Future;
using Utils;
using Ligric.Protobuf;
using Ligric.Business.Metadata;
using Ligric.Business.Extensions;
using Ligric.Business.Futures;
using static Ligric.Protobuf.BinanceFuturesOrders;
using Ligric.Business.Interfaces;
using System.Collections;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesOrdersService : IFuturesOrdersService, ISession
	{
		private int syncOrderChanged = 0;
		private CancellationTokenSource? _cts;
		private readonly Dictionary<long, FuturesOrderDto> _orders = new Dictionary<long, FuturesOrderDto>();
		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadataManager;
		private readonly BinanceFuturesOrdersClient _futuresClient;

		internal FuturesOrdersService(
			BinanceFuturesOrdersClient futuresClient,
			IMetadataManager metadataRepos,
			ICurrentUser currentUser)
		{
			_metadataManager = metadataRepos;
			_currentUser = currentUser;
			_futuresClient = futuresClient;
		}

		public IReadOnlyDictionary<long, FuturesOrderDto> Orders => new ReadOnlyDictionary<long, FuturesOrderDto>(_orders);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OrdersChanged;

		public Task AttachStreamAsync(long userApiId)
		{
			if (_cts != null && !_cts.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var userId = _currentUser.CurrentUser?.Id ?? throw new NullReferenceException("[AttachStream] UserId is null");

			var cts = new CancellationTokenSource();
			_cts = cts;
			return StreamApiSubscribeCall(userId, userApiId, cts.Token);
		}

		public void DetachStream()
		{
			StopStream();
			_orders.ClearAndRiseEvent(this, OrdersChanged, new Dictionary<long, FuturesOrderDto>(_orders), ref syncOrderChanged);
		}

		#region Session
		public void InitializeSession() { }

		public void ClearSession()
		{
			StopStream();
			_orders.ClearAndRiseEvent(this, OrdersChanged, new Dictionary<long, FuturesOrderDto>(_orders), ref syncOrderChanged);
			syncOrderChanged = 0;
		}

		public void Dispose()
		{
			StopStream();
		}
		#endregion

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
			lock (((ICollection)_orders).SyncRoot)
			{
				switch (api.Action)
				{
					case Protobuf.Action.Added:
						var exchangedOrderDto = api.Order.ToFuturesOrderDto();
						_orders.SetAndRiseEvent(this, OrdersChanged, api.Order.Id, exchangedOrderDto, ref syncOrderChanged);
						break;
					case Protobuf.Action.Removed:
						_orders.RemoveAndRiseEvent(this, OrdersChanged, api.Order.Id, ref syncOrderChanged);
						break;
					case Protobuf.Action.Changed:
						goto case Protobuf.Action.Added;
					case Protobuf.Action.Cleared:
						_orders.ClearAndRiseEvent(this, OrdersChanged, new Dictionary<long, FuturesOrderDto>(_orders), ref syncOrderChanged);
						break;
				}
			}
		}

		private void StopStream()
		{
			if (_cts != null)
			{
				_cts.Cancel();
				_cts.Dispose();
				_cts = null;
			}
		}
	}
}
