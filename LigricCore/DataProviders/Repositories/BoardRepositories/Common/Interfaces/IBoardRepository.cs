using Common.EventArgs;
using System;

namespace BoardRepositories.Interfaces
{
    public interface IBoardRepository<T>
    {
        event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;
    }
}
