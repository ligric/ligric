using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation.ElementsSeparatorExtentions
{
    internal static partial class ElementsSeparatorExtentions
    {
        public static FrameworkElement SetFront(this FrameworkElement element)
        {
            Panel parent = element.Parent as Panel;
            if (parent is null)
                throw new ArgumentNullException("Parent isn't a Panel");

            parent.Children.Remove(element);
            parent.Children.Add(element);
            return element;
        }
    }
}
