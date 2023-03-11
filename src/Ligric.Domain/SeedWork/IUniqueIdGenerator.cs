using System;

namespace Ligric.Domain.SeedWork
{
    public interface IUniqueIdGenerator
    {
        Guid GetUniqueId();
    }
}
