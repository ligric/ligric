using Grpc.Net.Client;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesCryptoClient : IFuturesCryptoClient
	{
		private readonly long userApi;

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

			var futuresClient = new FuturesClient(channel);
			_orders = new FuturesOrdersService(futuresClient, metadata, currentUser);
			_trades = new FuturesTradesService(futuresClient, metadata, currentUser);
			_positions = new FuturesPositionsService(futuresClient, metadata, currentUser);
			_leverages = new FuturesLeveragesService(futuresClient, metadata, currentUser);
		}

		public IFuturesOrdersService Orders => _orders;

		public IFuturesTradesService Trades => _trades;

		public IFuturesPositionsService Positions => _positions;

		public IFuturesLeveragesService Leverages => _leverages;

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
			Orders.Dispose();
			Trades.Dispose();
			Positions.Dispose();
			Leverages.Dispose();
		}
	}
}
