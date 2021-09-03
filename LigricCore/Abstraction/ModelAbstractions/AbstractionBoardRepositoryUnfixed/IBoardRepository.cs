namespace BoardRepository
{
    public interface IBoardRepository<T> : IDisposable
    {
        IEnumerable<T> GetAds();
    }
}
