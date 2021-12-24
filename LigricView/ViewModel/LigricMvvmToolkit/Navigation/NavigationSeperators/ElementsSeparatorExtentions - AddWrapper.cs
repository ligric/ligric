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

        public static Panel AddWrapper(this FrameworkElement element)
        {
            Panel wrapper = null;
            var parent = element.Parent;

            if (parent is UserControl)
            {
                wrapper = element.AddWrapperInTheUserControl((UserControl)parent);
            }
            else if (parent is Panel)
            {
                wrapper = element.AddWrapperInThePanel((Panel)parent);
            }
            else if(parent is Border)
            {
                wrapper = element.AddWrapperInTheBorder((Border)parent);
            }
            else if (parent is null)
            {
                throw new TypeAccessException("This element cannot be changed because his parent type is not allowed to page changes.");
            }
            else
            {
                throw new NotImplementedException($"Type {parent} not implemented");
            }

            return wrapper;
        }

        private static Panel AddWrapperInTheUserControl(this FrameworkElement element, UserControl parent)
        {
            Grid wrapper = new Grid();

            parent.Content = null;

            wrapper.Children.Add(element);

            parent.Content = wrapper;

            return wrapper;
        }

        private static Panel AddWrapperInThePanel(this FrameworkElement element, Panel parent)
        {
            Grid wrapper = new Grid();

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

        private static Panel AddWrapperInTheBorder(this FrameworkElement element, Border parent)
        {
            Grid wrapper = new Grid();

            parent.Child = null;

            wrapper.Children.Add(element);

            parent.Child = wrapper;

            return wrapper;
        }        

    }
}
