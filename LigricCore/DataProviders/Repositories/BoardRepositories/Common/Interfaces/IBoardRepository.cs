using Common.EventArgs;
using System;

namespace BoardRepositories.Interfaces
{
    public interface IBoardRepository<T>
    {
        public event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;
    }
}
