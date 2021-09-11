using BoardModels.BoardNotifications.Interfaces;
using BoardModels.Types;
using System.Collections.ObjectModel;

namespace BoardModels.BoardNotifications.Abstractions
{
    public abstract class AbstractBoardWithTimerNotifications : AbstractBoardModel, IAdDictionaryNotification, IBoardNameNotification, IBoardFiltersNotification
    {


        /// <summary>Инициализация оболочки <see cref="Ads"/>.</summary>
        protected AbstractBoardWithTimerNotifications()
        {
        }
    }
}
