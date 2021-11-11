using BoardModels.AbstractBoardNotifications.Interfaces;

namespace BoardModels.AbstractBoardNotifications.Abstractions
{
    public abstract class AbstractBoardWithTimerNotifications : AbstractBoardModel, IAdDictionaryNotification, IBoardNameNotification, IBoardFiltersNotification
    {
        /// <summary>Инициализация оболочки <see cref="Ads"/>.</summary>
        protected AbstractBoardWithTimerNotifications()
        {
        }
    }
}
