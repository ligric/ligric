using Utils;

namespace Ligric.Business.Interfaces.Futures
{
	public interface IFuturesCryptoManager
	{
		event EventHandler<NotifyDictionaryChangedEventArgs<long, IFuturesCryptoClient>>? ClientsChanged;

		void AddFuturesCryptoClient(long userApiId);

		void RemoveFuturesCryptoClient(long userApiId);
	}
}
