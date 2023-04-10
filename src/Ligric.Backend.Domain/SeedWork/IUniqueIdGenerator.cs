using System;

namespace Ligric.Backend.Domain.SeedWork
{
    public interface IUniqueIdGenerator
    {
        Guid GetUniqueId();
    }
}
