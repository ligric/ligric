using System;

namespace BoardsShared.BoardNotifications.Interfaces
{
    public delegate void ActionTimerIntervalHandler(object sender, TimeSpan time);

    public interface IBoardWithTimerNotification
    {
        event ActionTimerIntervalHandler TimerIntervalChanged;

        bool SetTimerInterval(TimeSpan time);
    }
}
