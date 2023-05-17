using System.Reflection;

namespace Ligric.UI.Helpers
{
	public class VisualTreeHelpers
	{
		/// <summary>
		/// Returns the first ancester of specified type
		/// </summary>
		public static T? FindAncestor<T>(DependencyObject current)
			where T : DependencyObject
		{
			current = VisualTreeHelper.GetParent(current);

			while (current != null)
			{
				if (current is T)
				{
					return (T)current;
				}
				current = VisualTreeHelper.GetParent(current);
			};
			return default;
		}

		public static void FindChildren<T>(List<T> results, DependencyObject startNode)
			where T : DependencyObject
		{
			int count = VisualTreeHelper.GetChildrenCount(startNode);
			for (int i = 0; i < count; i++)
			{
				DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
				if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
				{
					T asType = (T)current;
					results.Add(asType);
				}
				FindChildren<T>(results, current);
			}
		}

		public static void FindChildren<T>(List<T> results, DependencyObject startNode, string name)
			where T : DependencyObject
		{
			int count = VisualTreeHelper.GetChildrenCount(startNode);
			for (int i = 0; i < count; i++)
			{
				DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
				if (current is FrameworkElement frameworkElement && frameworkElement.Name == name
					&& (current.GetType().Equals(typeof(T))
					|| current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
				{
					T asType = (T)current;
					results.Add(asType);
				}
				FindChildren<T>(results, current, name);
			}
		}

		/// <summary>
		/// Finds an ancestor object by name and type
		/// </summary>
		public static T? FindAncestor<T>(DependencyObject current, string parentName)
			where T : DependencyObject
		{
			while (current != null)
			{
				if (!string.IsNullOrEmpty(parentName))
				{
					var frameworkElement = current as FrameworkElement;
					if (current is T && frameworkElement != null && frameworkElement.Name == parentName)
					{
						return (T)current;
					}
				}
				else if (current is T)
				{
					return (T)current;
				}
				current = VisualTreeHelper.GetParent(current);
			};

			return default;
		}

		/// <summary>
		/// Looks for a child control within a parent by name
		/// </summary>
		public static T? FindChild<T>(DependencyObject parent, string childName)
			where T : DependencyObject
		{
			// Confirm parent and childName are valid.
			if (parent == null)
			{
				return default;
			}

			T? foundChild = default;

			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				// If the child is not of the request child type child
				if (child is not T childType)
				{
					// recursively drill down the tree
					foundChild = FindChild<T>(child, childName);

					// If the child is found, break so we do not overwrite the found child.
					if (foundChild != null)
					{
						break;
					}
				}
				else if (!string.IsNullOrEmpty(childName))
				{
					var frameworkElement = child as FrameworkElement;
					// If the child's name is set for search
					if (frameworkElement != null && frameworkElement.Name == childName)
					{
						// if the child's name is of the request name
						foundChild = (T)child;
						break;
					}
					else
					{
						// recursively drill down the tree
						foundChild = FindChild<T>(child, childName);

						// If the child is found, break so we do not overwrite the found child.
						if (foundChild != null)
						{
							break;
						}
					}
				}
				else
				{
					// child element found.
					foundChild = (T)child;
					break;
				}
			}

			return foundChild;
		}

		/// <summary>
		/// Looks for a child control within a parent by type
		/// </summary>
		public static T? FindChild<T>(DependencyObject parent)
			where T : DependencyObject
		{
			// Confirm parent is valid.
			if (parent == null)
			{
				return default;
			}

			T? foundChild = default;

			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				// If the child is not of the request child type child
				if (child is not T childType)
				{
					// recursively drill down the tree
					foundChild = FindChild<T>(child);

					// If the child is found, break so we do not overwrite the found child.
					if (foundChild != null)
					{
						break;
					}
				}
				else
				{
					// child element found.
					foundChild = (T)child;
					break;
				}
			}
			return foundChild;
		}
	}
}
