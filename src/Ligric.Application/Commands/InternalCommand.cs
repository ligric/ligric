using MediatR;

namespace Ligric.Application.Commands
{
    public abstract class InternalCommand<TCommandResult> : IRequest<TCommandResult>
    {
    }
}