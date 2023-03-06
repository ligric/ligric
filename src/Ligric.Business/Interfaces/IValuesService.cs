using Ligric.Domain.Types.Future;
using System;
using System.Collections.Generic;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IValuesService : IDisposable
	{
		IReadOnlyDictionary<string, decimal> Values { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>> ValuesChanged;
	}
}
