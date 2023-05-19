using Ligric.Core.Types;
using Utils;

namespace Ligric.Core.Extensions
{
	public static class CoreTypeExtensions
	{
		public static NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>> ToIdentityNotifyDictionaryChangedEventArgs<TKey, TValue>(this NotifyDictionaryChangedEventArgs<TKey, TValue> args, Guid id)
		{
			return new NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>>(args.Action, args.Key, new IdentityEntity<TValue>(id, args.OldValue), new IdentityEntity<TValue>(id, args.NewValue), args.Number, args.SenderTime);
		}
	}
}
