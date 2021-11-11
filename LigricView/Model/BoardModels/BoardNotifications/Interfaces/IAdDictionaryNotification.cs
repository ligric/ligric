using BoardModels.AbstractBoardNotifications.Abstractions;
using Common.EventArgs;
using System;
using System.Collections.Generic;

namespace BoardModels.AbstractBoardNotifications.Interfaces
{
    public interface IAdDictionaryNotification
    {
        IReadOnlyDictionary<long, AdDto> Ads  { get; } 

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<long, AdDto>> AdsChanged;
    }
}
