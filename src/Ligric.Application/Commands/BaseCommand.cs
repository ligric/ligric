using MediatR;
using Ligric.Contracts.Helpers;

namespace Ligric.Application.Commands
{
    public abstract class BaseCommand : InternalCommand<CommandResult>
    {
    }
}