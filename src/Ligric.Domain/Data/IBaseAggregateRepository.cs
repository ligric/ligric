using System.Threading.Tasks;
using Ligric.Domain.Models;

namespace Ligric.Domain.Data
{
    public interface IBaseAggregateRepository<TEntity>: IBaseRepository<TEntity> where TEntity: AggregateRoot
    {
        Task ConcurrencySafeUpdate(TEntity entity, string loadedVersion);
    }
}
