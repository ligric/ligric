using LigricMvvmToolkit.Extantions;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    public static class ElementsSeparatorExtentions
    {
        private readonly static Type[] availableTypes = new Type[] { typeof(Grid), typeof(Border), typeof(Frame) };
        private static void AddGridWrapperInBorder(Border parent)
        {
            throw new NotImplementedException();
            var rootGrid = new Grid();

            parent.Child = null;

            rootGrid.Children.Add(parent);
        }

        public static FrameworkElement AddGridWrapper(this FrameworkElement element)
        {








            var parent = element.Parent;
            var test = parent.GetType();
            var parentType = availableTypes.FirstOrDefault(x => x == parent.GetType());

            if (parentType is null)
                throw new TypeAccessException("This element cannot be changed because his parent type is not allowed to page changes.");

            if (parentType == typeof(Border))
            {
                AddGridWrapperInBorder((Border)parent);
            }

            return element;
        }
        public static FrameworkElement RemoveGridWrapper(this FrameworkElement element)
        {
            throw new NotImplementedException();
            return element;
        }
        public static FrameworkElement AddElement(this FrameworkElement element, FrameworkElement addElement, out int index)
        {
            index = 2;
            
            return element;
        }
    }
}
