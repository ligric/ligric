using BoardsCore.CommonTypes.Entities;
using Common.EventArgs;
using System;
using System.Collections.Generic;

namespace BoardsCore.Abstractions.BoardAbstractions.Interfaces
{
    public interface IAdDictionaryNotification<TKey, TValue> where TValue : AdDto
    {
        IReadOnlyDictionary<TKey, TValue> Ads { get; }

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> AdsChanged;
    }
}
