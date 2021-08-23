using AbstractionBoardRepository.Interfaces;
using AbstractionRepository;
using Common.DtoTypes.Board;

namespace AbstractionBoardRepository.Abstracts
{
    public abstract class AbstractAdBoardWithTimerNotifications : AbstractAdBoardNotifications, IRepositoryTimerNotification
    {
        public readonly System.Timers.Timer Timer;
        public event ActionUpdateTimeStateHandler UpdateTimeChanged;

        protected virtual bool SetUpdateTimeAndSendAction(TimeSpan time)
        {
            if (Timer.Interval == time.TotalMilliseconds)
                return false;

            Timer.Interval = time.TotalMilliseconds;
            UpdateTimeChanged?.Invoke(this, time);

            return true;
        }

        protected abstract void RenderAds(object sender = null, System.Timers.ElapsedEventArgs e = null);

        public AbstractAdBoardWithTimerNotifications()
            : base()
        {
            Timer = new System.Timers.Timer();
            Timer.AutoReset = true;
            Timer.Interval = 15000;
            Timer.Elapsed += RenderAds;
        }

        protected AbstractAdBoardWithTimerNotifications(IDictionary<string, string> filters, TimeSpan updateTime)
            : base(filters)
        {
            Timer = new System.Timers.Timer();
            Timer.AutoReset = true;
            Timer.Interval = updateTime.TotalMilliseconds;
            Timer.Elapsed += RenderAds;
        }


        public AbstractAdBoardWithTimerNotifications(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState)
            : base(filters, repositoryState)
        {
            Timer = new System.Timers.Timer();
            Timer.AutoReset = true;
            Timer.Interval = updateTime.TotalMilliseconds;
            Timer.Elapsed += RenderAds;
        }
    }
}
