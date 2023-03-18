using System;

namespace Ligric.Service.AuthService.Application
{
    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }

        bool IsAvailable { get; }
    }
}
