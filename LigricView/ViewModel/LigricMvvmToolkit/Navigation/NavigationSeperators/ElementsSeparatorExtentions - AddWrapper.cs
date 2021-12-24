using LigricMvvmToolkit.Extantions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (parent is Control)
            {
                wrapper = element.AddWrapperInTheControl((Control)parent);
            }
            else if (parent is Panel)
            {
                wrapper = element.AddWrapperInThePanel((Panel)parent);
            }
            else if(parent is Border)
            {
                wrapper = AddWrapperInTheBorder((Border)parent);
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

        private static Panel AddWrapperInTheControl(this FrameworkElement element, Control parent)
        {
            Grid wrapper = new Grid() { Background = new SolidColorBrush(Colors.Blue), Width = 400 };

            var test = element;
            var removeElement = parent.GetVisualChild<Del>(element);

            wrapper.Children.Add(element);
            element = (FrameworkElement)wrapper.Children[0];

            return wrapper;
        }

        private static Panel AddWrapperInThePanel(this FrameworkElement element, Panel parent)
        {
            Grid wrapper = new Grid() { Background = new SolidColorBrush(Colors.Blue), Width = 400 };

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

        private static Panel AddWrapperInTheBorder(Border parent)
        {
            throw new NotImplementedException();
        }        

    }
}
