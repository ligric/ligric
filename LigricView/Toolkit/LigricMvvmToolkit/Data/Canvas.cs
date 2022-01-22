using LigricMvvmToolkit.Extensions;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Data
{
    public static partial class Canvas
    {
        #region Offset properties
        public static double GetOffsetX(UIElement element)
        {
            return (double)element.GetValue(OffsetXProperty);
        }

        public static void SetOffsetX(UIElement element, double value)
        {
            element.SetValue(OffsetXProperty, value);
        }

        // Using a DependencyProperty as the backing store for OffsetX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.RegisterAttached(nameof(GetOffsetX).Substring(3), typeof(double), typeof(Canvas), new PropertyMetadata(double.NaN, OffsetChanged));



        public static double GetOffsetY(UIElement element)
        {
            return (double)element.GetValue(OffsetYProperty);
        }

        public static void SetOffsetY(UIElement element, double value)
        {
            element.SetValue(OffsetYProperty, value);
        }

        // Using a DependencyProperty as the backing store for OffsetY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.RegisterAttached(nameof(GetOffsetY).Substring(3), typeof(double), typeof(Canvas), new PropertyMetadata(double.NaN, OffsetChanged));

        private static void OffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Отладочный вывод.
            Debug.WriteLine($"{nameof(Canvas)}.{e.Property}: {e.NewValue}");

            if (d.GetWithParent<Windows.UI.Xaml.Controls.Canvas>() is UIElement canvasItem)
            {
                if (e.Property == OffsetXProperty)
                    Windows.UI.Xaml.Controls.Canvas.SetLeft(canvasItem, (double)e.NewValue);
                else if (e.Property == OffsetYProperty)
                    Windows.UI.Xaml.Controls.Canvas.SetTop(canvasItem, (double)e.NewValue);
                else
                    throw new ArgumentException($"Неожидаемое свойство \"{e.Property}\"", "e.Property");
            }
            else if (d is FrameworkElement element && !element.IsLoaded)
            {
                element.Loaded -= OnLoaded;
                element.Loaded += OnLoaded;
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            double x = GetOffsetX(element);
            double y = GetOffsetY(element);

            if (element.GetWithParent<Windows.UI.Xaml.Controls.Canvas>() is UIElement canvasItem)
            {
                if (!double.IsNaN(x))
                    Windows.UI.Xaml.Controls.Canvas.SetLeft(canvasItem, x);
                if (!double.IsNaN(y))
                    Windows.UI.Xaml.Controls.Canvas.SetTop(canvasItem, y);
            }
        }

        #endregion
    }
}
