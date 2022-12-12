using Common.EventArgs;
using Ligric.Common.Types.Future;
using System;
using System.Collections.Generic;

namespace Ligric.Domain.Client.Base.Futures
{
    public interface IOrdersService
    {
        IReadOnlyDictionary<long, OpenOrderDto> OpenOrders { get; }

        event EventHandler<NotifyDictionaryChangedEventArgs<long, OpenOrderDto>> OpenOrdersChanged;
    }
}
