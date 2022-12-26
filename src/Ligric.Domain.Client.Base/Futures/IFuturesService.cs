using System.Collections.Generic;
using System;
using Ligric.Domain.Types.Future;
using Utils;

namespace Ligric.Domain.Client.Base.Futures
{
    public interface IFuturesService
    {
        IReadOnlyDictionary<long, PositionDto> Positions { get; }

        event EventHandler<NotifyDictionaryChangedEventArgs<long, PositionDto>> PositionsChanged;
    }
}
