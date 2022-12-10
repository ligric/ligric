using System;

namespace Ligric.Server.Domain.SeedWork
{
    public interface IUniqueIdGenerator
    {
        Guid GetUniqueId();
    }
}
