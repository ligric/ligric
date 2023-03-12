using System.Threading.Tasks;
using Ligric.Service.AuthService.Domain.Model.MessageBroker;

namespace Ligric.Service.AuthService.Application.Contracts.Services
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T> (MessageBody<T>  message);
    }
}
