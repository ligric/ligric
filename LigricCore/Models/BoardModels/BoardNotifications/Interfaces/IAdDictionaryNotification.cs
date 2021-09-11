using BoardModels.Types;
using Common.EventArgs;

namespace BoardModels.BoardNotifications.Interfaces
{
    public interface IAdDictionaryNotification
    {
        IReadOnlyDictionary<long, AbsctractAdDtoType> Ads  { get; } 

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<long, AbsctractAdDtoType>> AdsChanged;
    }
}
