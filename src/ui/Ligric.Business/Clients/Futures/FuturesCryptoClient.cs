using Grpc.Net.Client;
using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Clients.Apies;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients.Futures
{
	public class FuturesCryptoClient : ILigricCryptoClient
	{
		private readonly long userApi;

		private readonly IAuthorizationService _authorization;

		public FuturesCryptoClient(
			GrpcChannel channel,
			IAuthorizationService authorization,
			IMetadataManager metadata)
		{
			_authorization = authorization;

			Apis = new ApiesService(channel, metadata, _authorization);

			var futuresClient = new FuturesClient(channel);
			Orders = new FuturesOrdersService(futuresClient, metadata, _authorization);
			Values = new FuturesTradesService(futuresClient, metadata, _authorization);
			Positions = new FuturesPositionsService(futuresClient, metadata, _authorization);
			Leverages = new FuturesLeveragesService(futuresClient, metadata, _authorization);

			_authorization.AuthorizationStateChanged += OnAuthorizationStateChanged;
		}

		public IApiesService Apis { get; }

		public IFuturesOrdersService Orders { get; }

		public IFuturesTradesService Values { get; }

		public IFuturesPositionsService Positions { get; }

		public IFuturesLeveragesService Leverages { get; }

		public void Dispose()
		{
			_authorization.AuthorizationStateChanged -= OnAuthorizationStateChanged;
			Apis.Dispose();
			Orders.Dispose();
			Values.Dispose();
			Positions.Dispose();
			Leverages.Dispose();
		}

		private void OnAuthorizationStateChanged(object sender, Core.Types.User.UserAuthorizationState e)
		{
			switch (e)
			{
				case Core.Types.User.UserAuthorizationState.Connected:
					Apis.ApiPiplineSubscribeAsync();
					break;
				case Core.Types.User.UserAuthorizationState.Disconnected:
					Apis.ApiPiplineUnsubscribe();
					break;
			}
		}
	}
}
