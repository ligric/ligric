using Grpc.Net.Client;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using Ligric.Core.Extensions;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Utils;
using static Ligric.Protobuf.Futures;

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

			var futuresClient = new FuturesClient(channel);
			_orders = new FuturesOrdersService(futuresClient, metadata, currentUser);
			_trades = new FuturesTradesService(futuresClient, metadata, currentUser);
			_positions = new FuturesPositionsService(futuresClient, metadata, currentUser);
			_leverages = new FuturesLeveragesService(futuresClient, metadata, currentUser);

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

		public async Task AttachStreamAsync()
		{
			await _orders.AttachStreamAsync(userApi);
			await _trades.AttachStreamAsync(userApi);
			await _positions.AttachStreamAsync(userApi);
			await _leverages.AttachStreamAsync(userApi);
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
