using Windows.UI.Xaml;

namespace DragPosition
{
    public partial class DragPosition
    {
        /// <summary>Возвращает значение присоединённого свойства IsDrop для <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> значение свойства которого будет возвращено.</param>
        /// <returns><see cref="bool"/> значение свойства.</returns>
        public static bool GetIsDrop(UIElement element)
        {
            return (bool)element.GetValue(IsDropProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства IsDrop для <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="bool"/> значение для свойства.</param>
        public static void SetIsDrop(UIElement element, bool value)
        {
            element.SetValue(IsDropProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> для методов <see cref="GetIsDrop(UIElement)"/> и <see cref="SetIsDrop(UIElement, bool)"/>.</summary>
        public static readonly DependencyProperty IsDropProperty =
            DependencyProperty.RegisterAttached(nameof(GetIsDrop).Substring(3), typeof(bool), typeof(DragPosition), new PropertyMetadata(false, IsDropChanged));

        private static void IsDropChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            UIElement element = (UIElement)dependencyObject;
            HandlersData data = GetHandlersData(element);
            if ((bool)args.NewValue)
            {
                if (data == null)
                {
                    SetHandlersData(element, new HandlersData(element/*, parent*/));
                }
            }
            else
            {
                if (data != null)
                {
                    data.Dispose();
                    element.ClearValue(IsDropProperty);
                }
            }
        }
    }
}
