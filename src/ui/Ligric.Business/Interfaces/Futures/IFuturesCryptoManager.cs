using System.Collections.ObjectModel;
using Utils;

namespace Ligric.Business.Interfaces.Futures
{
	public interface IFuturesCryptoManager
	{
		public ReadOnlyDictionary<long, IFuturesCryptoClient> Clients { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, IFuturesCryptoClient>>? ClientsChanged;

		void AddFuturesCryptoClient(long userApiId);

		void RemoveFuturesCryptoClient(long userApiId);
	}
}
