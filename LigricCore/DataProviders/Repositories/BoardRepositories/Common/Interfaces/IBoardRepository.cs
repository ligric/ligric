using Common.EventArgs;
using System;

namespace BoardRepositories.Interfaces
{
    public interface IBoardRepository<TKey, TEntity>
    {
        event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TEntity>> AdsChanged;
    }
}
