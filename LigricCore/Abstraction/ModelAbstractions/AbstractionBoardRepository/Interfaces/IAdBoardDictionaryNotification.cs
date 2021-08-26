using Common.DtoTypes.Board;
using Common.EventArgs;

namespace AbstractionBoardRepository.Interfaces
{
    public interface IAdBoardDictionaryNotification
    {
        IReadOnlyDictionary<long, AdDto> Ads { get; }

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<long, AdDto>> AdsChanged;
    }
}
