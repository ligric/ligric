using Common.DtoTypes.Board;
using Common.EventArgs;

namespace AbstractionBoardRepository.Interfaces
{
    public interface IAdBoardDictionaryNotification
    {
        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyEnumerableChangedEventArgs<AdDto>> AdsChanged;
    }
}
