using Common.Delegates;
using Common.Enums;

namespace BoardModels.AbstractBoardNotifications.Interfaces
{
    public interface IBoardStateNotification
    {
        StateEnum CurrentState { get; }

        event ActionStateHandler BoardStateChanged;
    }
}
