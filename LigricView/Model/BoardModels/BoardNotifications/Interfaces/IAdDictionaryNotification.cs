using BoardModels.CommonTypes.Entities;
using Common.EventArgs;
using System;
using System.Collections.Generic;

namespace BoardModels.AbstractBoardNotifications.Interfaces
{
    public interface IAdDictionaryNotification<T> where T : AdDto
    {
        IReadOnlyDictionary<long, T> Ads  { get; } 

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<long, T>> AdsChanged;
    }
}
