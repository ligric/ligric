﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace Ligric.Business.Extensions
{
	internal static class NotifyCollectionChangedEventHandlerExtensions
	{
		public static bool AddAndRiseEvent<T>(this ICollection<T> collection, object sender, T element, NotifyCollectionChangedEventHandler? handler)
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

		public static bool RemoveAndRiseEvent<T>(this ICollection<T> collection, object sender, T element, NotifyCollectionChangedEventHandler? handler)
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
	}
}
