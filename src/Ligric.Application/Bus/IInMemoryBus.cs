using System.Threading.Tasks;
using Ligric.Application.Commands;
using Ligric.Application.Queries;
using Ligric.Contracts.Helpers;
using Ligric.Domain.Models;

namespace Ligric.Application.Bus
{
    /// <summary>
    /// It is used as a mediator to send and handle requests inside a single service
    /// </summary>
    public interface IInMemoryBus
    {
        Task<Result<CommandResult>> SendCommand<TCommand>(TCommand cmd) where TCommand : BaseCommand;
        Task<Result<TQueryResult>> SendQuery<TQueryResult>(BaseQuery<TQueryResult> query);
        Task PublishEvent(DomainEvent @event);
    }
}
