﻿using Common.EventArgs;
using Ligric.Common.Types.Future;
using System;
using System.Collections.Generic;

namespace Ligric.Common.Abstractions.Futures
{
    public interface IFuturesProvider
    {
        IReadOnlyDictionary<long, PositionDto> Positions { get; }

        IReadOnlyDictionary<long, OpenOrderDto> OpenOrders { get; }

        event EventHandler<NotifyDictionaryChangedEventArgs<long, PositionDto>> FuturesChanged;

        event EventHandler<NotifyDictionaryChangedEventArgs<long, OpenOrderDto>> OrdersChanged;
    }
}
