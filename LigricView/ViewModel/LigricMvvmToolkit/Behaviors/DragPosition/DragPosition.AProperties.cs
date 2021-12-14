using Microsoft.Xaml.Interactivity;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.Behaviors.DragPosition
{
    [Bindable]
    public partial class DragPosition
    {
        #region DragPosition
        public static DragPositionData GetDragPosition(UIElement element)
        {
            return (DragPositionData)element.GetValue(DragPositionProperty);
        }

        public static void SetDragPosition(UIElement element, DragPositionData value)
        {
            element.SetValue(DragPositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for DragPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragPositionProperty =
            DependencyProperty.RegisterAttached("DragPosition", typeof(DragPositionData), typeof(DragPosition), new PropertyMetadata(null, DragPositionChanged));

        private static void DragPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = (UIElement)d;
            DragPositionData data = (DragPositionData)e.NewValue;
            if (data.BaseParent is UIElement parent)
                SetBaseParent(element, parent);
            else
                BindingOperations.SetBinding(element, BaseParentProperty, (BindingBase)data.BaseParent);

            if (data.OffsetX is double x)
                SetOffsetX(element, x);
            else
                BindingOperations.SetBinding(element, OffsetXProperty, (BindingBase)data.OffsetX);

            if (data.OffsetY is double y)
                SetOffsetY(element, y);
            else
                BindingOperations.SetBinding(element, OffsetYProperty, (BindingBase)data.OffsetY);

            data.BindingAction?.Invoke(element);

            HandlersData handlersData = new HandlersData(element, GetBaseParent(element));
            SetHandlersData(element, handlersData);
        }


        #endregion


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
            DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(DragPosition), new PropertyMetadata(0.0));



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
            DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(DragPositionData), new PropertyMetadata(0.0));

        #endregion

        #region BaseParent


        /// <summary>Возвращает значение присоединённого свойства BaseParent для <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> значение свойства которого будет возвращено.</param>
        /// <returns><see cref="UIElement"/> значение свойства.</returns>
        public static UIElement GetBaseParent(UIElement element)
        {
            return (UIElement)element.GetValue(BaseParentProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства BaseParent для <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="UIElement"/> значение для свойства.</param>
        public static void SetBaseParent(UIElement element, UIElement value)
        {
            element.SetValue(BaseParentProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> для методов <see cref="GetBaseParent(UIElement)"/> и <see cref="SetBaseParent(UIElement, UIElement)"/>.</summary>
        public static readonly DependencyProperty BaseParentProperty =
            DependencyProperty.RegisterAttached(nameof(GetBaseParent).Substring(3), typeof(UIElement), typeof(DragPosition), new PropertyMetadata(null));


        #endregion
    }
}
