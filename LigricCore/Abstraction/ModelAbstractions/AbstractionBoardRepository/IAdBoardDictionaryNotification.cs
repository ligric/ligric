using Common.DtoTypes.Board;
using System;
using System.Collections.Generic;

namespace AbstractionBoardRepository
{
    public interface IAdBoardDictionaryNotification
    {
        IReadOnlyDictionary<long, AdReadOnlyStruct> Ads { get; }

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<long, AdReadOnlyStruct>> AdsChanged;
    }
}
