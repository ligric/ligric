namespace BoardRepository.Interfaces
{
    public enum StateEnum
    {
        Active,
        Stoped
    }

    public delegate void ActionStateHandler(object sender, StateEnum state);

    public interface IStateNotification
    {
        StateEnum CurrentState { get; }
        event ActionStateHandler StateChanged;
        bool SetState(StateEnum state);
    }
}
