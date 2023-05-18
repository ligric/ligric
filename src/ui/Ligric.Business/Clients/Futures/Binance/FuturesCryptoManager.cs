using System.Collections.ObjectModel;
using Grpc.Net.Client;
using Ligric.Business.Interfaces;
using Ligric.Business.Interfaces.Futures;
using Ligric.Business.Metadata;
using Utils;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesCryptoManager : IFuturesCryptoManager
	{
		private int sync = 0;
		private readonly GrpcChannel _channel;
		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadata;

		private readonly Dictionary<long, IFuturesCryptoClient> _clients = new Dictionary<long, IFuturesCryptoClient>();

		public FuturesCryptoManager(
			GrpcChannel channel,
			ICurrentUser currentUser, // Temporary
			IMetadataManager metadata)
		{
			_channel = channel;
			_currentUser = currentUser;
			_metadata = metadata;
		}

		public ReadOnlyDictionary<long, IFuturesCryptoClient> Clients => new ReadOnlyDictionary<long, IFuturesCryptoClient>(_clients);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, IFuturesCryptoClient>>? ClientsChanged;

		public void AddFuturesCryptoClient(long userApiId)
		{
			var client = new FuturesCryptoClient(_channel, _currentUser, _metadata, userApiId);
			_clients.AddAndRiseEvent(this, ClientsChanged, userApiId, client, ref sync);
		}

		public void RemoveFuturesCryptoClient(long userApiId)
		{
			_clients.RemoveAndRiseEvent(this, ClientsChanged, userApiId, ref sync);
		}
	}
}
