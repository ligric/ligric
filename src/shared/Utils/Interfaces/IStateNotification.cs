namespace Utils
{
    public interface IStateNotification
    {
        StateEnum CurrentState { get; }
        event ActionStateHandler StateChanged;
        bool SetState(StateEnum state);
    }
}
