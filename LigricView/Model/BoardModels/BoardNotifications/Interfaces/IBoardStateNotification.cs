using Common.Delegates;
using Common.Enums;

namespace BoardsShared.AbstractBoardNotifications.Interfaces
{
    public interface IBoardStateNotification
    {
        StateEnum CurrentState { get; }

        event ActionStateHandler BoardStateChanged;
    }
}
