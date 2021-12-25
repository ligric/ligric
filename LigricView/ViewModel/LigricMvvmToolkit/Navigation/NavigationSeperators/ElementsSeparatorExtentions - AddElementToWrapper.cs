using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    public static partial class ElementsSeparatorExtentions
    {
        public static FrameworkElement AddElementToWrapper(this Panel wrapper, FrameworkElement addElement)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("Wrapper element is null");
            }

            wrapper.Children.Add(addElement);

            return addElement;
        }
    }
}
