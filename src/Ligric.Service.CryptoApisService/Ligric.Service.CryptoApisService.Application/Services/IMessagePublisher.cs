using System.Threading.Tasks;
using Ligric.Service.CryptoApisService.Domain.Model.MessageBroker;

namespace Ligric.Service.CryptoApisService.Application.Services
{
	public interface IMessageProducer
	{
		Task SendMessageAsync<T>(MessageBody<T> message);
	}
}
