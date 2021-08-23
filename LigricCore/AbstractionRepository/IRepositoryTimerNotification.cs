namespace AbstractionRepository
{
    public delegate void ActionUpdateTimeStateHandler(object sender, TimeSpan time);
    public interface IRepositoryTimerNotification
    {
        event ActionUpdateTimeStateHandler UpdateTimeChanged;
    }
}
