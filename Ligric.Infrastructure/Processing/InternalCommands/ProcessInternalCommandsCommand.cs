using MediatR;
using Ligric.Application;
using Ligric.Application.Configuration.Commands;
using Ligric.Infrastructure.Processing.Outbox;

namespace Ligric.Infrastructure.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}