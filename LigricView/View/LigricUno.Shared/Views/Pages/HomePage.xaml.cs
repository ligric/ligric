using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LigricUno.Views.Pages
{
    // TODO : TEST
    internal static class Asffsaf
    {
        public static T FindVisualParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            // get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return default(T);

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
    }

    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        // TODO : TEST
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Надо создать новую Grid third
            Grid third = new Grid() { Background = new SolidColorBrush(Colors.Blue), Width = 400 };

            Panel parent = (second.Parent == null ? second.FindVisualParent<Panel>() : second.Parent) as Panel;
            if (parent == null)
                return;

            // Удалить second из first.Children
            foreach (var item in parent.Children)
            {
                if (item == second)
                {
                    parent.Children.Remove(item);
                }
            }

            // Добавить second в third.Children
            third.Children.Add(second);

            // Добавить third в first.Children
            parent.Children.Add(third);
        }
    }
}
