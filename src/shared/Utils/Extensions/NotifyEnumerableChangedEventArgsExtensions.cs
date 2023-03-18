using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class NotifyEnumerableChangedEventArgsExtensions
    {
        public static void NewElementsHandler<TKey, TValue>(this IDictionary<TKey, TValue> currentEntities, object sender,
            IDictionary<TKey, TValue> newEntities, EventHandler<NotifyEnumerableChangedEventArgs<TValue>> entitiesChanged, ref int actionNumber)
        {
            lock (((ICollection)currentEntities).SyncRoot)
            {
                if (newEntities.Count <= 0)
                {
                    currentEntities.Clear();
                    entitiesChanged?.Invoke(sender, NotifyActionEnumerableChangedEventArgs.Reset(new TValue[0], actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
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
                            entitiesChanged?.Invoke(sender, NotifyActionEnumerableChangedEventArgs.Changed(oldEntity, newEntity, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                        }
                        newEntities.Remove(entityKey);
                    }
                    else
                    {
                        var removedEntity = currentEntities[entityKey];
                        currentEntities.Remove(entityKey);
                        entitiesChanged?.Invoke(sender, NotifyActionEnumerableChangedEventArgs.Removed(removedEntity, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    }
                }

                //add new items
                if (newEntities.Count > 0)
                {
                    foreach (var newItemPair in newEntities)
                    {
                        currentEntities.Add(newItemPair.Key, newItemPair.Value);
                        entitiesChanged?.Invoke(sender, NotifyActionEnumerableChangedEventArgs.Added(newItemPair.Value, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    }
                }
            }
        }
    }
}
