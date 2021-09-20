using Common.EventArgs;

namespace BoardRepository.Interfaces
{
    public interface IBoardRepository<T>
    {
        public event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;
    }
}
