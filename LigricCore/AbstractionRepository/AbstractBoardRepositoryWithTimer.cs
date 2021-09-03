namespace BoardRepository
{
    public abstract class AbstractBoardRepositoryWithTimer<T> : AbstractBoardRepository<T>, IRepositoryWithTimerNotification
    {
        protected readonly System.Timers.Timer timer;

        public event ActionTimerIntervalHandler TimerIntervalChanged;

        public virtual bool SetTimerInterval(TimeSpan time)
        {
            if (timer.Interval == time.TotalMilliseconds)
                return false;

            timer.Interval = time.TotalMilliseconds;
            TimerIntervalChanged?.Invoke(this, time);

            return true;
        }

        protected abstract void RenderAds(object sender = null, System.Timers.ElapsedEventArgs e = null);

        public AbstractBoardRepositoryWithTimer(RepositoryStateEnum repositoryState = RepositoryStateEnum.Stoped)
            : base(repositoryState)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 15000;
            timer.Elapsed += RenderAds;
        }

        public AbstractBoardRepositoryWithTimer(TimeSpan timerInterval, RepositoryStateEnum repositoryState = RepositoryStateEnum.Stoped)
            : base(repositoryState)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = timerInterval.TotalMilliseconds;
            timer.Elapsed += RenderAds;
            timer.Start();
        }

    }
}
