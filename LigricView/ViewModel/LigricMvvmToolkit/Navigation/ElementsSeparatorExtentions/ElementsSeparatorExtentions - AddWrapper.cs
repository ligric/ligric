using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtentions
    {
        public static Panel AddWrapper(this FrameworkElement element, string rootKey = "root")
        {
            var parent = element.Parent;
            if (parent is null)
                throw new TypeAccessException("Root parent element is null.");

            if (string.IsNullOrEmpty(rootKey))
                throw new ArgumentNullException("Root key cannot be null or empty.");

            if (wrappers.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
                return wrapperInfo.Wrapper;


            Panel wrapper = new Grid() { Tag = rootKey + "_wrapper" };
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

            wrappers.Add(rootKey, new WrapperInfo(rootKey, wrapper, 0));

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
