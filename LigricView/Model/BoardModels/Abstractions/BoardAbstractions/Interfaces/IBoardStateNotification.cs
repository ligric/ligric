using Common.Delegates;
using Common.Enums;

namespace BoardsCore.Abstractions.BoardAbstractions.Interfaces
{
    public interface IBoardStateNotification
    {
        StateEnum CurrentState { get; }

        event ActionStateHandler BoardStateChanged;
    }
}
