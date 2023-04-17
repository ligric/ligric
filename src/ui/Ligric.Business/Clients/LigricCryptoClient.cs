using System;
using Grpc.Net.Client;
using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Clients.Apies;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
//using static Ligric.Protobuf.Futures;

namespace Ligric.Business.Clients
{
	public class LigricCryptoClient : ILigricCryptoClient
	{
		private readonly IAuthorizationService _authorization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public LigricCryptoClient(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
			GrpcChannel channel,
			IAuthorizationService authorization,
		    IMetadataManager metadata)
		{
			_authorization = authorization;

			Apis = new ApiesService(channel, metadata, _authorization);

			//FuturesClient futuresClient = new FuturesClient(channel); 
			//Orders = new OrdersService(futuresClient, metadata, _authorization);
			//Values = new ValuesService(futuresClient, metadata, _authorization);
			//Positions = new PositionsService(futuresClient, metadata, _authorization);

			_authorization.AuthorizationStateChanged += OnAuthorizationStateChanged;
		}

		public IApiesService Apis { get; }

		public IOrdersService Orders { get; }

		public IValuesService Values { get; }

		public IPositionsService Positions { get; }

		public void Dispose() => throw new NotImplementedException();

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
