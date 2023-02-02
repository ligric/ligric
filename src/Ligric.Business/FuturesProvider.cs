﻿using Ligric.Domain.Client.Base.Futures;
using Ligric.Domain.Types.Future;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils;

namespace Ligric.Business
{
    public class FuturesProvider : IFuturesProvider
    {
        private readonly Dictionary<long, PositionDto> _positions = new Dictionary<long, PositionDto>();
        private readonly Dictionary<long, OpenOrderDto> _openOrders = new Dictionary<long, OpenOrderDto>();

        public IReadOnlyDictionary<long, PositionDto> Positions => new ReadOnlyDictionary<long, PositionDto>(_positions);

        public IReadOnlyDictionary<long, OpenOrderDto> OpenOrders => new ReadOnlyDictionary<long, OpenOrderDto>(_openOrders);

        //public event EventHandler<NotifyDictionaryChangedEventArgs<long, PositionDto>> PositionsChanged;
        //public event EventHandler<NotifyDictionaryChangedEventArgs<long, OpenOrderDto>> OpenOrdersChanged;
    }
}
