using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ligric.UI.Helpers
{
	public static class UIHelper
	{
		/// <summary>
		/// Finds a parent of a given item on the visual tree.
		/// </summary>
		/// <typeparam name="T">The type of the queried item.</typeparam>
		/// <param name="child">A direct or indirect child of the queried item.</param>
		/// <returns>The first parent item that matches the submitted type parameter. 
		/// If not matching item can be found, a null reference is being returned.</returns>
		public static T? FindVisualParent<T>(this DependencyObject child)
			where T : DependencyObject
		{
			// get parent item
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);

			// we’ve reached the end of the tree
			if (parentObject == null) return default;

			// check if the parent matches the type we’re looking for
			if (parentObject is T parent && parent != null)
			{
				return parent;
			}
			else
			{
				// use recursion to proceed with next level
				return FindVisualParent<T>(parentObject);
			}
		}

		public static UIElement? FindVisualParent(this UIElement element, Type type)
		{
			UIElement? parent = element;
			while (parent != null)
			{
				if (type.IsAssignableFrom(parent.GetType()))
				{
					return parent;
				}
				parent = VisualTreeHelper.GetParent(parent) as UIElement;
			}
			return null;
		}

		/// <summary>
		/// Will navigate down the VisualTree to find an element that is of the provided type.
		/// </summary>
		/// <typeparam name="T">The type of object to search for</typeparam>
		/// <param name="element">The element to start searching at</param>
		/// <returns>The found child or null if not found</returns>
		public static T? GetVisualChild<T>(this DependencyObject element) where T : DependencyObject
		{
			T? child = default;
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);

			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject obj = VisualTreeHelper.GetChild(element, i);
				if (obj is T)
				{
					child = (T)obj;
					break;
				}
				else
				{
					child = GetVisualChild<T>(obj);
					if (child != null)
						break;
				}
			}

			return child;
		}
	}
}
