namespace BoardRepository
{
    public delegate void ActionTimerIntervalHandler(object sender, TimeSpan time);

    public interface IRepositoryWithTimerNotification
    {
        event ActionTimerIntervalHandler TimerIntervalChanged;
        bool SetTimerInterval(TimeSpan time);
    }
}
