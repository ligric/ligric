using Ligric.Types.Future;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IPositionsService : IDisposable 
	{
		IReadOnlyDictionary<long, FuturesPositionDto> Positions { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesPositionDto>> PositionsChanged;

		Task AttachStreamAsync(long userApiId);

		void DetachStream();
	}
}
