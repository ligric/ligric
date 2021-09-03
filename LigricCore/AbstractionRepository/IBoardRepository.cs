using Common.EventArgs;

namespace BoardRepository
{
    public interface IBoardRepository<T>
    {
        public event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;
    }
}
