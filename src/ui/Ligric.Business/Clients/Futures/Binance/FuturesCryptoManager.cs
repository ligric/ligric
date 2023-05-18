using System.Collections.ObjectModel;
using Grpc.Net.Client;
using Ligric.Business.Apies;
using Ligric.Business.Interfaces;
using Ligric.Business.Interfaces.Futures;
using Ligric.Business.Metadata;
using Utils;
using Ligric.Core.Ligric.Core.Types.Api;

namespace Ligric.Business.Clients.Futures.Binance
{
	public class FuturesCryptoManager : IFuturesCryptoManager
	{
		private int sync = 0;
		private readonly GrpcChannel _channel;
		private readonly ICurrentUser _currentUser;
		private readonly IMetadataManager _metadata;
		private readonly IApiesService _apisService;

		private readonly Dictionary<long, IFuturesCryptoClient> _clients = new Dictionary<long, IFuturesCryptoClient>();

		public FuturesCryptoManager(
			GrpcChannel channel,
			ICurrentUser currentUser, // Temporary
			IMetadataManager metadata,
			IApiesService apisService)
		{
			_channel = channel;
			_currentUser = currentUser;
			_metadata = metadata;
			_apisService = apisService;
			_apisService.ApiesChanged += OnApiesChanged;
		}

		public ReadOnlyDictionary<long, IFuturesCryptoClient> Clients => new ReadOnlyDictionary<long, IFuturesCryptoClient>(_clients);

		public event EventHandler<NotifyDictionaryChangedEventArgs<long, IFuturesCryptoClient>>? ClientsChanged;

		private void OnApiesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					foreach (var item in e.NewItems)
					{
						var api = (ApiClientDto)item;
						AddFuturesCryptoClient((long)api.UserApiId!);
					}
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
					foreach (var item in e.OldItems)
					{
						var api = (ApiClientDto)item;
						RemoveFuturesCryptoClient((long)api.UserApiId!);
					}
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
					ClearFuturesCryptoClients();
					break;
				default: throw new NotImplementedException();
			}
		}

		private void AddFuturesCryptoClient(long userApiId)
		{
			var client = new FuturesCryptoClient(_channel, _currentUser, _metadata, userApiId);
			_clients.AddAndRiseEvent(this, ClientsChanged, userApiId, client, ref sync);
		}

		private void RemoveFuturesCryptoClient(long userApiId)
		{
			if (_clients.TryGetValue(userApiId, out var client))
			{
				client.DetachStream(); // Raise collection clear event
				client.Dispose(); // befor dispose
				_clients.RemoveAndRiseEvent(this, ClientsChanged, userApiId, ref sync);
			}
		}

		private void ClearFuturesCryptoClients()
		{
			foreach (var client in _clients.Values)
			{
				client.DetachStream();
				client.Dispose();
			}
			_clients.ClearAndRiseEvent(this, ClientsChanged, ref sync);
		}
	}
}
