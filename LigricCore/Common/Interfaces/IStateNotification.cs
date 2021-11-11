using Common.Delegates;
using Common.Enums;

namespace Common.Interfaces
{
    public interface IStateNotification
    {
        StateEnum CurrentState { get; }
        event ActionStateHandler StateChanged;
        bool SetState(StateEnum state);
    }
}
