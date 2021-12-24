using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    public static partial class ElementsSeparatorExtentions
    {
        public static FrameworkElement AddElementToWrapper(this Panel root, FrameworkElement addElement)
        {
            if (root == null)
            {
                throw new ArgumentNullException("Wrapper element is null");
            }

            root.Children.Add(addElement);

            return addElement;
        }
    }
}
