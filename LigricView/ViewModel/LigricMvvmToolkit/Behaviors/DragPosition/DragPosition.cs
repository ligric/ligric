using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Behaviors.DragPosition
{
    public partial class DragPosition 
    {

        /// <summary>Возвращает значение присоединённого свойства HandlersData для <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> значение свойства которого будет возвращено.</param>
        /// <returns><see cref="HandlersData"/> значение свойства.</returns>
        private static HandlersData GetHandlersData(UIElement element)
        {
            return (HandlersData)element.GetValue(HandlersDataProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства HandlersData для <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="HandlersData"/> значение для свойства.</param>
        private static void SetHandlersData(UIElement element, HandlersData value)
        {
            element.SetValue(HandlersDataProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> для методов <see cref="GetHandlersData(UIElement)"/> и <see cref="SetHandlersData(UIElement, HandlersData)"/>.</summary>
        private static readonly DependencyProperty HandlersDataProperty =
            DependencyProperty.RegisterAttached(nameof(GetHandlersData).Substring(3), typeof(HandlersData), typeof(DragPosition),
                new PropertyMetadata(null, HandlersDataChanged));

        private static void HandlersDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = (UIElement)d;
            if(e.OldValue is HandlersData handlersData)
            {
                handlersData.Dispose();
            }
        }
    }
}
