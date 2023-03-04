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
		private readonly Dictionary<long, FuturesOrderDto> _openOrders = new Dictionary<long, FuturesOrderDto>();

		public OrdersService(GrpcChannel channel)
		{

		}

		public IReadOnlyDictionary<long, FuturesOrderDto> OpenOrders => new ReadOnlyDictionary<long, FuturesOrderDto>(_openOrders);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>? OpenOrdersChanged;

		public Task AttachStreamAsync() => throw new NotImplementedException();
		public void DetachStream() => throw new NotImplementedException();
		public void Dispose()
		{
			// Just for disable warning
			OnOrdersChanged();
		}
		public Task SubscribeOpenOrdersFromUserIdAsync(long userApiId) => throw new NotImplementedException();
		public Task UnsubscribeOpenOrdersFromUserIdAsync(long userApiId) => throw new NotImplementedException();

		private void OnOrdersChanged()
		{
			var futuresDto = new FuturesOrderDto(0, "asfsa", Domain.Types.Side.Buy, 123, 123, 123);
			OpenOrdersChanged?.Invoke(null, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair<long, FuturesOrderDto>(0, futuresDto, 0, 0));
		}
	}
}
