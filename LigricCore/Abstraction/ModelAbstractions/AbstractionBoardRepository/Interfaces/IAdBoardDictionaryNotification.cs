using Common.DtoTypes.Board;

namespace AbstractionBoardRepository.Interfaces
{
    public interface IAdBoardDictionaryNotification
    {
        IReadOnlyDictionary<long, AdReadOnlyStruct> Ads { get; }

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<long, AdReadOnlyStruct>> AdsChanged;
    }
}
