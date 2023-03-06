using System;
using Grpc.Net.Client;
using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Clients.Apies;
using Ligric.Business.Clients.Futures;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using Ligric.Protos;

namespace Ligric.Business.Clients
{
	public class LigricCryptoClient : ILigricCryptoClient
	{
		private readonly IAuthorizationService _authorization;

		public LigricCryptoClient(
			GrpcChannel channel,
			IAuthorizationService authorization,
		    IMetadataManager metadata)
		{
			_authorization = authorization;
			Apis = new ApiesService(channel, metadata, _authorization);
			Orders = new OrdersService(channel, metadata, _authorization);
			Values = new ValuesService(channel, metadata, _authorization);

			_authorization.AuthorizationStateChanged += OnAuthorizationStateChanged;
		}

		public IApiesService Apis { get; }

		public IOrdersService Orders { get; }

		public IValuesService Values { get; }

		public void Dispose() => throw new NotImplementedException();

		private void OnAuthorizationStateChanged(object sender, Domain.Types.User.UserAuthorizationState e)
		{
		}
	}
}
