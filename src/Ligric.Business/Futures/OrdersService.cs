using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Ligric.Domain.Types.Future;
using Utils;

namespace Ligric.Business.Futures
{
	public class OrdersService : IOrdersService
	{
		private readonly Dictionary<long, OpenOrderDto> _openOrders = new Dictionary<long, OpenOrderDto>();

		public OrdersService(GrpcChannel channel)
		{

		}

		public IReadOnlyDictionary<long, OpenOrderDto> OpenOrders => new ReadOnlyDictionary<long, OpenOrderDto>(_openOrders);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, OpenOrderDto>>? OpenOrdersChanged;

		public Task AttachStreamAsync() => throw new NotImplementedException();
		public void DetachStream() => throw new NotImplementedException();
		public void Dispose() => throw new NotImplementedException();
		public Task SubscribeOpenOrdersFromUserIdAsync(long userApiId) => throw new NotImplementedException();
		public Task UnsubscribeOpenOrdersFromUserIdAsync(long userApiId) => throw new NotImplementedException();
	}
}
