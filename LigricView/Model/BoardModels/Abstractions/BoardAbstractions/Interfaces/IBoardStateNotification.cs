using Common.Delegates;
using Common.Enums;

namespace BoardsShared.Abstractions.BoardAbstractions.Interfaces
{
    public interface IBoardStateNotification
    {
        StateEnum CurrentState { get; }

        event ActionStateHandler BoardStateChanged;
    }
}
