using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ligric.Application.Configuration.Commands;
using Ligric.Domain.SeedWork;
using Ligric.Infrastructure.Database;

namespace Ligric.Infrastructure.Processing
{
    public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T:ICommand
    {
        private readonly ICommandHandler<T> _decorated;

        private readonly IUnitOfWork _unitOfWork;

        private readonly DevPaceContext _devPaceContext;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated, 
            IUnitOfWork unitOfWork,
            DevPaceContext devPaceContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _devPaceContext = devPaceContext;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                var internalCommand =
                    await _devPaceContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id,
                        cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}