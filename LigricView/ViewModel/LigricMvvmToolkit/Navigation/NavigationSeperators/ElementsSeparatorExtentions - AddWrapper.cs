using LigricMvvmToolkit.Extantions;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LigricMvvmToolkit.Navigation
{
    public static partial class ElementsSeparatorExtentions
    {
        public static FrameworkElement RemoveGridWrapper(this FrameworkElement element)
        {
            throw new NotImplementedException();
            return element;
        }

        public static Panel AddWrapper(this FrameworkElement element, string rootKey = "root")
        {
            var parent = element.Parent;
            if (parent is null)
                throw new TypeAccessException("Root parent element is null.");

            if (string.IsNullOrEmpty(rootKey))
                throw new ArgumentNullException("Root key cannot be null or empty.");


            if (!wrappers.TryGetValue(rootKey, out Panel wrapper))
            {
                wrapper = new Grid() { Tag = rootKey + "_wrapper" };
            }
            else
            {
                return wrapper;
            }

            if (parent is UserControl)
            {
                wrapper = element.AddWrapperInTheUserControl((UserControl)parent, wrapper);
            }
            else if (parent is Panel)
            {
                wrapper = element.AddWrapperInThePanel((Panel)parent, wrapper);
            }
            else if(parent is Border)
            {
                wrapper = element.AddWrapperInTheBorder((Border)parent, wrapper);
            }
            else
            {
                throw new NotImplementedException($"Type {parent} not implemented");
            }

            wrappers.Add(rootKey, wrapper);

            return wrapper;
        }

        private static Panel AddWrapperInTheUserControl(this FrameworkElement element, UserControl parent, Panel wrapper)
        {
            parent.Content = null;

            wrapper.Children.Add(element);

            parent.Content = wrapper;

            return wrapper;
        }

        private static Panel AddWrapperInThePanel(this FrameworkElement element, Panel parent, Panel wrapper)
        {
            foreach (var item in parent.Children)
            {
                if (item == element)
                {
                    parent.Children.Remove(item);
                }
            }

            wrapper.Children.Add(element);

            parent.Children.Add(wrapper);

            return wrapper;
        }

        private static Panel AddWrapperInTheBorder(this FrameworkElement element, Border parent, Panel wrapper)
        {
            parent.Child = null;

            wrapper.Children.Add(element);

            parent.Child = wrapper;

            return wrapper;
        }        

    }
}
