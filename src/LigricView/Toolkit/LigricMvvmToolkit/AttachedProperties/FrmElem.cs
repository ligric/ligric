using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricMvvmToolkit.AttachedProperties
{
    public static class FrmElem
    {

        /// <summary>Получение заданной пропорции отношения ширины к высоте элемента.
        /// Задаётся коэффициент для получения размера высоты из ширины: Height = Width * WidthToHeight.</summary>
        /// <param name="element">FrameworkElement чьи размеры должны быть пропорциональны.</param>
        public static double GetWidthToHeight(FrameworkElement element)
        {
            return (double)element.GetValue(WidthToHeightProperty);
        }

        /// <summary>Задание пропорции отношения ширины к высоте элемента.
        /// Задаётся коэффициент для получения размера высоты из ширины: Height = Width * WidthToHeight.</summary>
        /// <param name="element">FrameworkElement чьи размеры должны быть пропорциональны.</param>
        /// <param name="widthToHeight">Коэфициент пропорции:  Height = Width * WidthToHeight.</param>
        /// <returns>Отслеживается изменение размера контейнера предоставленного элемента.
        /// При его изменении определяется максимально возможный размер элемента который может
        /// быть достигнут в контейнере при соблюдении заданных пропорций.</returns>
        public static void SetWidthToHeight(FrameworkElement element, double widthToHeight)
        {
            element.SetValue(WidthToHeightProperty, widthToHeight);
        }

        // Using a DependencyProperty as the backing store for Proportionate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthToHeightProperty =
            DependencyProperty.RegisterAttached("WidthToHeight", typeof(double), typeof(FrmElem), new PropertyMetadata(-1.0, ProportionalChanged));

        /// <summary>Метод обратного вызова после изменения значения свойства.</summary>
        /// <param name="d">FrameworkElement иначе исключение.</param>
        /// <param name="e">Параметры изменения</param>
        private static void ProportionalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                throw new ArgumentException("Must be a FrameworkElement");

            /// Получение элемента сохраняющего пропорции.
            SizeChangedElement changedElement = GetSizeChangeElement(element);

            /// Если элемент не задан, то его создание и сохранение.
            if (changedElement == null)
                SetSizeChangeElement(element, changedElement = new SizeChangedElement(element));

            /// Передача нового значения пропорции.
            changedElement.SetWidthToHeight((double)e.NewValue);
        }

        /// <summary>Приватный геттер AP-свойства SizeChangeElementPropertyKey.</summary>
        private static SizeChangedElement GetSizeChangeElement(FrameworkElement obj)
        {
            return (SizeChangedElement)obj.GetValue(SizeChangeElementProperty);
        }

        /// <summary>Приватный сеттер AP-свойства SizeChangeElementPropertyKey.</summary>
        private static void SetSizeChangeElement(FrameworkElement obj, SizeChangedElement value)
        {
            obj.SetValue(SizeChangeElementProperty, value);
        }

        // Using a DependencyProperty as the backing store for SizeChangeElement.  This enables animation, styling, binding, etc...
        /// <summary>Регистрация приватного AP-свойства только для чтения.</summary>
        private static readonly DependencyProperty SizeChangeElementProperty = DependencyProperty.RegisterAttached("SizeChangeElement", typeof(SizeChangedElement), typeof(FrmElem), new PropertyMetadata(null));

        /// <summary>Вспомогательный приватный класс отслеживающий изменения размера элемента.</summary>
        private class SizeChangedElement
        {
            /// <summary>FrameworkElement. Не может быть null.</summary>
            public FrameworkElement Element { get; }

            /// <summary>Задётся коэфициент для получения размера высоты из ширины: Height = Width * WidthToHeight.
            /// Если равен или меньше нуля, то соблюдение пропорции не производится.</summary>
            public double WidthToHeight { get; private set; } = 1;

            /// <summary>Конструктор принимающий элемент чьи пропорции не должны меняться.</summary>
            /// <param name="element">FrameworkElement. Не может быть null.</param>
            public SizeChangedElement(FrameworkElement element)
            {
                Element = element ?? throw new ArgumentNullException(nameof(element));

                /// Событие происходит при изменении выделяемого для элемента места.
                element.LayoutUpdated += Element_LayoutUpdated;
            }

            /// <summary>Обработчик вызываемый при изменении выделяемого для элемента места.</summary>
            /// <param name="sender"><see langword="null"/>.</param>
            /// <param name="e">Empty or <see langword="null"/>.</param>
            private void Element_LayoutUpdated(object sender = null, object e = null)
            {
                if (WidthToHeight <= 0)
                    return;

                // Получение информации о выделенном месте.
                Rect rect = LayoutInformation.GetLayoutSlot(Element);

                // Размеры выделенной области с учётом Margin элемента.
                double widthArea = rect.Width - Element.Margin.Left - Element.Margin.Right;
                double heightArea = rect.Height - Element.Margin.Top - Element.Margin.Bottom;

                // > 0 ? widthArea : 1
                double width = widthArea;
                double height = width * WidthToHeight;
                if (height > heightArea)
                {
                    height = heightArea;
                    width = height / WidthToHeight;
                }
                Element.Width = width > 0.0 ? width : 0.0;
                Element.Height = height > 0.0 ? height : 0.0;
            }

            /// <summary>Задание коэффициента пропорции ширины к высоте.</summary>
            /// <param name="widthToHeight">Коэффициент пропорции ширины к высоте: Height = Width * WidthToHeight.</param>
            public void SetWidthToHeight(double widthToHeight)
            {
                WidthToHeight = widthToHeight;
                Element_LayoutUpdated();
            }

        }

    }
}
