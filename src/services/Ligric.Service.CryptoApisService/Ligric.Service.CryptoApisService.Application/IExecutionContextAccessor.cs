using System;

namespace Ligric.Service.CryptoApisService.Application
{
    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }

        bool IsAvailable { get; }
    }
}
