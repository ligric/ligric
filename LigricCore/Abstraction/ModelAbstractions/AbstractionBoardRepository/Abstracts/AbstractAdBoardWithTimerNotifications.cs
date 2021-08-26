using AbstractionBitZlatoRequests;
using AbstractionBoardRepository.Interfaces;
using AbstractionRepository;
using System.ComponentModel;

namespace AbstractionBoardRepository.Abstracts
{
    public abstract class AbstractAdBoardWithTimerNotifications : AbstractAdBoardNotifications, IRepositoryTimerNotification, ISupportInitializeBoardRepository
    {
        protected readonly System.Timers.Timer timer;
        protected IBitZlatoRequestsService bitZlatoRequests;

        public event ActionUpdateTimeStateHandler UpdateTimeChanged;

        protected virtual bool SetUpdateTimeAndSendAction(TimeSpan time)
        {
            if (timer.Interval == time.TotalMilliseconds)
                return false;

            timer.Interval = time.TotalMilliseconds;
            UpdateTimeChanged?.Invoke(this, time);

            return true;
        }

        protected abstract void RenderAds(object sender = null, System.Timers.ElapsedEventArgs e = null);

        #region Constructors
        public AbstractAdBoardWithTimerNotifications()
            : base()
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 15000;
            timer.Elapsed += RenderAds;
        }

        public AbstractAdBoardWithTimerNotifications(IDictionary<string, string> filters, TimeSpan updateTime)
            : base(filters)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = updateTime.TotalMilliseconds;
            timer.Elapsed += RenderAds;
        }

        public AbstractAdBoardWithTimerNotifications(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState)
            : base(filters, repositoryState)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = updateTime.TotalMilliseconds;
            timer.Elapsed += RenderAds;

            ((ISupportInitializeBoardRepository)this).Initialize(filters, repositoryState);
        }

        public AbstractAdBoardWithTimerNotifications(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState, IBitZlatoRequestsService bitZlatoRequests)
            : base(filters, repositoryState)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = updateTime.TotalMilliseconds;
            timer.Elapsed += RenderAds;

            ((ISupportInitializeBoardRepository)this).Initialize(filters, repositoryState, bitZlatoRequests);
        }
        #endregion

        #region ISupportInitializeBoardRepository
        public bool IsBeginInit { get; protected set; }

        public void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            initializeRates.EndInit();
        }

        public void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, string name)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            SetNameAndSendAction(name);
            initializeRates.EndInit();
        }

        public void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, IBitZlatoRequestsService bitZlatoRequests)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            this.bitZlatoRequests = bitZlatoRequests;
            initializeRates.EndInit();
        }

        public void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, string name, IBitZlatoRequestsService bitZlatoRequests)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            SetNameAndSendAction(name);

            this.bitZlatoRequests = bitZlatoRequests;
            initializeRates.EndInit();
        }

        public void BeginInit()
        {
            if (IsBeginInit)
                throw new MethodAccessException("Нельзя начинать новую инициализацию пока не закончена предыдущая транзакциия инициализации.");

            IsBeginInit = true;
            timer.Stop();
        }

        public void EndInit()
        {
            if (!IsBeginInit)
                throw new MethodAccessException("Транзакциия инициализации не была начата.");

            IsBeginInit = false;
            RenderAds();
        }
        #endregion
    }
}
