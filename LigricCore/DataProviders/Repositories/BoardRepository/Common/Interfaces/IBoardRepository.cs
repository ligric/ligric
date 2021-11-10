using Common.EventArgs;
using System;

namespace BoardRepository.Interfaces
{
    public interface IBoardRepository<T>
    {
        public event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;
    }
}
