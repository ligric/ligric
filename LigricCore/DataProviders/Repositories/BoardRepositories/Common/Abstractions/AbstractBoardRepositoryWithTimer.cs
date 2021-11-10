using BoardRepositories.Interfaces;
using Common.Interfaces;
using System;
using System.Collections.Generic;

namespace BoardRepositories.Abstractions
{
    public abstract partial class AbstractBoardRepositoryWithTimer<T> : AbstractBoardRepository<T>, IRepositoryWithTimerNotification, ISupportInitializeBoardRepository
    {
        #region Timer notification
        protected readonly System.Timers.Timer timer = new System.Timers.Timer();

        public virtual bool SetTimerInterval(TimeSpan newInterval)
        {
            TimeSpan oldInterval = TimeSpan.FromMilliseconds(timer.Interval);

            if (oldInterval == newInterval)
                return false;

            timer.Interval = newInterval.TotalMilliseconds;
            RaiseActionTimerInterval(newInterval);
            return true;
        }

        protected abstract void RenderAds(object sender = null, System.Timers.ElapsedEventArgs e = null);
        #endregion

        #region Methods
        public override bool SetState(StateEnum state)
        {
            StateEnum old = CurrentState;
            if (state == old)
                return false;

            if (state == StateEnum.Active)
                timer.Start();
            else
                timer.Stop();

            CurrentState = state;
            RaiseActionState(state);
            return true;
        }
        #endregion

        #region ISupportInitialize
        public override void BeginInit()
        {
            // TODO : Exception
            if (IsBeginInit)
                throw new MethodAccessException("Нельзя начинать новую инициализацию пока не закончена предыдущая транзакциия инициализации.");

            IsBeginInit = true;
            timer.Stop();
        }

        public override void EndInit()
        {
            // TODO : Exception
            if (!IsBeginInit)
                throw new MethodAccessException("Транзакциия инициализации не была начата.");

            IsBeginInit = false;
            RenderAds();
        }
        #endregion

        #region Constructors
        public AbstractBoardRepositoryWithTimer(StateEnum state = StateEnum.Stoped)
            :base(state)
        {
            timer.AutoReset = true;
            timer.Interval = 15000;
            timer.Elapsed += RenderAds;
        }
        public AbstractBoardRepositoryWithTimer(IDictionary<string, string> parameters, StateEnum state = StateEnum.Stoped)
            :base(parameters, state)
        {
            timer.AutoReset = true;
            timer.Interval = 15000;
            timer.Elapsed += RenderAds;
        }

        public AbstractBoardRepositoryWithTimer(TimeSpan timerInterval, StateEnum state = StateEnum.Stoped)
            : base(state)
        {
            timer.AutoReset = true;
            timer.Interval = timerInterval.TotalMilliseconds;
            timer.Elapsed += RenderAds;
        }

        public AbstractBoardRepositoryWithTimer(TimeSpan timerInterval, IDictionary<string, string> parameters, StateEnum state = StateEnum.Stoped)
            : base(parameters, state)
        {
            timer.AutoReset = true;
            timer.Interval = timerInterval.TotalMilliseconds;
            timer.Elapsed += RenderAds;
        }
        #endregion

    }
}
