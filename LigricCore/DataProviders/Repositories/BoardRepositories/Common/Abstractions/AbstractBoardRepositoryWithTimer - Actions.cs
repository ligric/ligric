using BoardRepositories.Interfaces;
using System;

namespace BoardRepositories.Abstractions
{
    public abstract partial class AbstractBoardRepositoryWithTimer<TKey, TEntity>
    {
        public event ActionTimerIntervalHandler TimerIntervalChanged;
        protected void RaiseActionTimerInterval(TimeSpan time)
            => TimerIntervalChanged?.Invoke(this, time);
    }
}
