using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace Ligric.Business.Futures
{
	public interface IValuesService : IDisposable
	{
		IReadOnlyDictionary<string, decimal> Values { get; }

		event EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>> ValuesChanged;

		Task AttachStreamAsync(long userApiId);
	}
}
