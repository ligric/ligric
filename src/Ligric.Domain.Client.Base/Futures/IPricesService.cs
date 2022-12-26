using Ligric.Domain.Types.Future;
using System;
using System.Collections.Generic;
using Utils;

namespace Ligric.Domain.Client.Base.Futures
{
    public interface IPricesService
    {
        IReadOnlyDictionary<string, PriceDto> Prices { get; }

        event EventHandler<NotifyDictionaryChangedEventArgs<string, PriceDto>> PricesChanged;
    }
}
