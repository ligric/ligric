using MediatR;
using Ligric.Application;
using Ligric.Application.Configuration.Commands;

namespace Ligric.Infrastructure.Processing.Outbox
{
    public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}