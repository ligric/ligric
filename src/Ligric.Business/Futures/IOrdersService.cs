using Ligric.Domain.Types.Future;
using System;
using System.Collections.Generic;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IOrdersService
	{
		IReadOnlyDictionary<long, OpenOrderDto> OpenOrders { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, OpenOrderDto>> OpenOrdersChanged;
	}
}
