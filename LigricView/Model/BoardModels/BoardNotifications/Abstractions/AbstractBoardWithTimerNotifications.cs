using BoardModels.AbstractBoardNotifications.Interfaces;
using BoardModels.CommonTypes.Entities;

namespace BoardModels.AbstractBoardNotifications.Abstractions
{
    public abstract class AbstractBoardWithTimerNotifications<T> : AbstractBoardModel<T>, IAdDictionaryNotification<T>, IBoardNameNotification, IBoardFiltersNotification
        where T : AdDto
    {
        /// <summary>Инициализация оболочки <see cref="Ads"/>.</summary>
        protected AbstractBoardWithTimerNotifications()
        {
        }
    }
}
