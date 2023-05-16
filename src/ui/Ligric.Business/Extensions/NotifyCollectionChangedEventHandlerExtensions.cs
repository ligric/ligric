using System.Collections.Specialized;

namespace Ligric.Business.Extensions
{
	internal static class NotifyCollectionChangedEventHandlerExtensions
	{
		public static bool AddAndRiseEvent<T>(this ICollection<T> collection, object sender, NotifyCollectionChangedEventHandler? handler, T element)
			where T : notnull
		{ 
			if (collection.Contains(element))
			{
				return false;
			}
			collection.Add(element);
			handler?.Invoke(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, element));
			return true;
		}

		public static bool RemoveAndRiseEvent<T>(this ICollection<T> collection, object sender, NotifyCollectionChangedEventHandler? handler, T element)
			where T : notnull
		{
			if (!collection.Contains(element))
			{
				return false;
			}
			collection.Remove(element);
			handler?.Invoke(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, element));
			return true;
		}

		public static void UpdateAndRiseEvent<T>(this HashSet<T> collection, object sender, T oldElement, T newElement, NotifyCollectionChangedEventHandler? handler)
			where T : notnull
		{
			collection.Remove(oldElement);
			collection.Add(newElement);
			handler?.Invoke(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldElement, newElement));
		}

		public static void ResetAndRiseEvent<T>(this ICollection<T> collection, object sender, NotifyCollectionChangedEventHandler? handler)
			where T : notnull
		{
			if (collection.Count == 0) return;

			collection.Clear();
			handler?.Invoke(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
