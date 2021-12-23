using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LigricUno.Views.Pages
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();

            //UIElementCollection uiElements = second.Children;


            // Надо создать новую Grid third
            Grid third = new Grid() { Background = new SolidColorBrush(Colors.Blue), Width= 600 };

            // Удалить second из first.Children
            foreach (var item in first.Children)
            {
                if (item == second)
                {
                    first.Children.Remove(item);
                }
            }

            // Добавить second в third.Children
            third.Children.Add(second);

            // Добавить third в first.Children
            first.Children.Add(third);


            //foreach (var item in second.)
            //{

            //}

            //foreach (var item in uiElements)
            //{
            //    grid2.Children.Add(item);
            //}

            //second.Children.Clear();

            //foreach (var item in grid2.Children)
            //{
            //    second.Children.Add(item);
            //}
            //grid2.Children.Add(new Button() { Content = "Second button", Width = 300, Height = 70 });
        } 


    }
}
