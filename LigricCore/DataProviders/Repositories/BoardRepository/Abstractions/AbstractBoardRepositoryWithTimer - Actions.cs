using BoardRepository.Interfaces;

namespace BoardRepository.Abstractions
{
    public abstract partial class AbstractBoardRepositoryWithTimer<T>
    {
        public event ActionTimerIntervalHandler TimerIntervalChanged;
        protected void RaiseActionTimerInterval(TimeSpan time)
            => TimerIntervalChanged?.Invoke(this, time);
    }
}
