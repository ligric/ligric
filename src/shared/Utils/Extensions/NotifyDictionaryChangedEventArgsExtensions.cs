﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class NotifyDictionaryChangedEventArgsExtensions
    {
        public static void NewElementsHandler<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender,
            IDictionary<TKey, TValue> newEntities, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, ref int actionNumber)
        {
            lock (((ICollection)currentEntities).SyncRoot)
            {
                if (newEntities.Count <= 0)
                {
                    currentEntities.Clear();
                    action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<TKey, TValue>(actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    return;
                }

                //remove old items and change new items
                for (int i = currentEntities.Count - 1; i >= 0; i--)
                {
                    var index = i;
                    var entityKey = currentEntities.Keys.ElementAt(index);
                    if (newEntities.TryGetValue(entityKey, out var newEntity))
                    {
                        var oldEntity = currentEntities[entityKey];
                        if (!Equals(oldEntity, newEntity))
                        {
                            currentEntities[entityKey] = newEntity;
                            action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(entityKey, oldEntity, newEntity, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                        }
                        newEntities.Remove(entityKey);
                    }
                    else
                    {
                        currentEntities.Remove(entityKey);
                        action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<TKey, TValue>(entityKey, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    }
                }

                //add new items
                if (newEntities.Count > 0)
                {
                    foreach (var newItemPair in newEntities)
                    {
                        currentEntities.Add(newItemPair.Key, newItemPair.Value);
                        action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(newItemPair.Key, newItemPair.Value, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    }
                }
            }
        }

        ///<summary>Добавления в словарь новой пары: ключ-значение.
        /// Возвращает false, если такой ключ уже есть и добавление не было выполнено.</summary>
        public static bool AddAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, TKey addKey, TValue addValue, ref int actionNumber)
        {
            if (!currentEntities.TryAdd(addKey, addValue))
                return false;

            action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(addKey, addValue, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            return true;
        }

#if NETSTANDARD2_0
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }

            return false;
        }
#endif

        ///<summary>Удаление из словаря пары: ключ-значение.
        /// Возвращает false, если такого ключа нет и удаление не было выполнено.</summary>
        public static bool RemoveAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, TKey removeKey, ref int actionNumber)
        {
            if (currentEntities.Remove(removeKey))
            {
                action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<TKey, TValue>(removeKey, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                return true;
            }
            return false;

        }

		///<summary>Удаление из словаря пары: ключ-значение.
		/// Возвращает false, если такого ключа нет и удаление не было выполнено.</summary>
		public static bool RemoveAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, TKey removeKey, TValue oldValue, ref int actionNumber)
		{
			if (currentEntities.Remove(removeKey))
			{
				action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<TKey, TValue>(removeKey, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds(), oldValue));
				return true;
			}
			return false;

		}

		///<summary> Задание в словаре значения ключу.
		/// Возвращает false, если такого ключа нет и вместо замены было выполнено добавление.</summary>
		public static bool SetAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, TKey changeKey, TValue changeValue, ref int actionNumber)
        {
            if (currentEntities.TryGetValue(changeKey, out TValue oldValue))
            {
                currentEntities[changeKey] = changeValue;
                action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(changeKey, oldValue, changeValue, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                return true;
            }

            currentEntities.Add(changeKey, changeValue);
            action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(changeKey, changeValue, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            return false;
        }

		/// <summary> Задание в словаре значения ключу.</summary>
		/// <returns><see cref="NotifyDictionaryChangedAction.Added"/> if value was added.<br/>
		/// <see cref="NotifyDictionaryChangedAction.Changed"/> if item was changed.<br/>
		/// <see cref="Nullable"/> result if nothing was happend.</returns>
		/// <remarks> <paramref name="changeValue"/> cannot be null </remarks>
		public static NotifyDictionaryChangedAction? EqualBeforeAddOrSetAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, TKey changeKey, TValue changeValue, ref int actionNumber)
		{
			if (currentEntities.TryGetValue(changeKey, out TValue oldValue))
			{
				if (oldValue == null || !oldValue.Equals(changeValue))
				{
					currentEntities[changeKey] = changeValue;
					action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(changeKey, oldValue, changeValue, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
					return NotifyDictionaryChangedAction.Changed;
				}
				return null;
			}

			currentEntities.Add(changeKey, changeValue);
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(changeKey, changeValue, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
			return NotifyDictionaryChangedAction.Added;
		}

		///<summary>Очистка словаря.
		/// Возвращает false, если словарь был пустой.</summary>
		public static bool ClearAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, ref int actionNumber)
        {
            var isEmpty = currentEntities.Count != 0;

            if (isEmpty)
            {
                currentEntities.Clear();
                action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<TKey, TValue>(actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                return true;
            }
            return false;
        }

		public static bool ClearAndRiseEvent<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender, EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? action, IDictionary<TKey, TValue> oldItems, ref int actionNumber)
        {
            var isEmpty = currentEntities.Count != 0;

            if (isEmpty)
            {
                currentEntities.Clear();
                action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<TKey, TValue>(actionNumber++, oldItems, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                return true;
            }
            return false;
        }
    }
}
