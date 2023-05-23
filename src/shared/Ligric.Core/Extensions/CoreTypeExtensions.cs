using System.Linq;
using Ligric.Core.Types;
using Utils;

namespace Ligric.Core.Extensions
{
	public static class CoreTypeExtensions
	{
		public static NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>> ToIdentityNotifyDictionaryChangedEventArgs<TKey, TValue>(this NotifyDictionaryChangedEventArgs<TKey, TValue> args, Guid id)
			=> args.Action switch
			{
				NotifyDictionaryChangedAction.Added => new NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>>(
					args.Action, args.Key, new IdentityEntity<TValue>(id, args.OldValue), new IdentityEntity<TValue>(id, args.NewValue), args.Number, args.SenderTime),
				NotifyDictionaryChangedAction.Removed => new NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>>(
					args.Action, args.Key, new IdentityEntity<TValue>(id, args.OldValue), new IdentityEntity<TValue>(id, args.NewValue), args.Number, args.SenderTime),
				NotifyDictionaryChangedAction.Changed => new NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>>(
					args.Action, args.Key, new IdentityEntity<TValue>(id, args.OldValue), new IdentityEntity<TValue>(id, args.NewValue), args.Number, args.SenderTime),
				NotifyDictionaryChangedAction.Cleared => new NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>>(
					args.Action,
					null,
					args.OldDictionary!.Select(oldValue => new KeyValuePair<TKey, IdentityEntity<TValue>>(oldValue.Key, new IdentityEntity<TValue>(id, oldValue.Value))).ToDictionary(x => x.Key, x => x.Value),
					args.Number, args.SenderTime),
				NotifyDictionaryChangedAction.Initialized => new NotifyDictionaryChangedEventArgs<TKey, IdentityEntity<TValue>>(
					args.Action, args.Key, new IdentityEntity<TValue>(id, args.OldValue), new IdentityEntity<TValue>(id, args.NewValue), args.Number, args.SenderTime),
				_ => throw new NotImplementedException()
			};
	}
}
