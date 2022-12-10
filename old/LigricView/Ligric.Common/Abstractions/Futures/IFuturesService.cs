using Ligric.Common.Types.Future;
using System.Collections.Generic;
using System;
using Common.EventArgs;

namespace Ligric.Common.Abstractions.Futures
{
    public interface IFuturesService
    {
        IReadOnlyDictionary<long, PositionDto> Positions { get; }

        event EventHandler<NotifyDictionaryChangedEventArgs<long, PositionDto>> PositionsChanged;
    }
}
