namespace Ligric.Infrastructure.Caching
{
    public interface ICacheStoreItem
    {
        string CacheKey { get; }
    }
}