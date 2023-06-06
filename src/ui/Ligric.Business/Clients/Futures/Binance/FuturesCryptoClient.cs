using Grpc.Net.Client;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using Ligric.Core.Extensions;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;
using Utils;
using static Ligric.Protobuf.BinanceFuturesLeverages;
using static Ligric.Protobuf.BinanceFuturesOrders;
using static Ligric.Protobuf.BinanceFuturesPositions;
using static Ligric.Protobuf.BinanceFuturesTrades;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesCryptoClient : IFuturesCryptoClient
	{
		private readonly long userApi;
		public Guid ClientId { get; }

		private readonly FuturesOrdersService _orders;
		private readonly FuturesTradesService _trades;
		private readonly FuturesPositionsService _positions;
		private readonly FuturesLeveragesService _leverages;

		public FuturesCryptoClient(
			GrpcChannel channel,
			ICurrentUser currentUser,
			IMetadataManager metadata,
			long userApi)
		{
			this.userApi = userApi;
			this.ClientId = Guid.NewGuid();

			_orders = new FuturesOrdersService(new BinanceFuturesOrdersClient(channel), metadata, currentUser);
			_trades = new FuturesTradesService(new BinanceFuturesTradesClient(channel), metadata, currentUser);
			_positions = new FuturesPositionsService(new BinanceFuturesPositionsClient(channel), metadata, currentUser);
			_leverages = new FuturesLeveragesService(new BinanceFuturesLeveragesClient(channel), metadata, currentUser);

			_orders.OrdersChanged += OnOrdersChanged;
			_positions.PositionsChanged += OnPositionsChanged;
			_leverages.LeveragesChanged += OnLeveragesChanged;
		}

		public IFuturesOrdersService Orders => _orders;

		public IFuturesTradesService Trades => _trades;

		public IFuturesPositionsService Positions => _positions;

		public IFuturesLeveragesService Leverages => _leverages;

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesOrderDto>>>? ClientOrdersChanged;

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, IdentityEntity<FuturesPositionDto>>>? ClientPositionsChanged;

		public event EventHandler<NotifyDictionaryChangedEventArgs<string, IdentityEntity<LeverageDto>>>? ClientLeveragesChanged;

		private void OnOrdersChanged(object sender, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> e)
			=> ClientOrdersChanged?.Invoke(this, e.ToIdentityNotifyDictionaryChangedEventArgs(ClientId));

		private void OnPositionsChanged(object sender, NotifyDictionaryChangedEventArgs<long, FuturesPositionDto> e)
			=> ClientPositionsChanged?.Invoke(this, e.ToIdentityNotifyDictionaryChangedEventArgs(ClientId));

		private void OnLeveragesChanged(object sender, NotifyDictionaryChangedEventArgs<string, LeverageDto> e)
			=> ClientLeveragesChanged?.Invoke(this, e.ToIdentityNotifyDictionaryChangedEventArgs(ClientId));

		public void AttachStream()
		{
			_orders.AttachStreamAsync(userApi);
			_trades.AttachStreamAsync(userApi);
			_positions.AttachStreamAsync(userApi);
			_leverages.AttachStreamAsync(userApi);
		}

		public void DetachStream()
		{
			_orders.DetachStream();
			_trades.DetachStream();
			_positions.DetachStream();
			_leverages.DetachStream();
		}

		public void Dispose()
		{
			_orders.OrdersChanged -= OnOrdersChanged;
			_positions.PositionsChanged -= OnPositionsChanged;
			_leverages.LeveragesChanged -= OnLeveragesChanged;

			Orders.Dispose();
			Trades.Dispose();
			Positions.Dispose();
			Leverages.Dispose();
		}
	}
}
