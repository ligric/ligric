using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.AttachedProperties.DragPosition
{
    /// <summary>Основные данные для создания привязок в DragPositionBehavior.</summary>
    public class DragPositionData
    {
        private object _baseParent;
        private object _offsetX;
        private object _offsetY;

        private static readonly DoubleConverter doubleConverter = new DoubleConverter();
        public static bool DoubleTryParse(object value, out object result)
        {
            if (value is double dble)
            {
                result = dble;
                return true;
            }

            if (value == null || !doubleConverter.IsValid(value))
            {
                result = null;
                return false;
            }

            result = doubleConverter.ConvertFrom(value);
            return true;
        }

        public object BaseParent
        {
            get => _baseParent;
            set
            {
                if (value is UIElement || value is BindingBase)
                    _baseParent = value;
                else
                    throw new ArgumentException(nameof(value));
            }
        }
        public object OffsetX
        {
            get => _offsetX;
            set
            {
                if (value is BindingBase ||
                    DoubleTryParse(value, out value))
                    _offsetX = value;
                else
                    throw new ArgumentException(nameof(value));
            }
        }
        public object OffsetY
        {
            get => _offsetY;
            set
            {
                if (value is BindingBase ||
                    DoubleTryParse(value, out value))
                    _offsetY = value;
                else
                    throw new ArgumentException(nameof(value));
            }
        }

        public Action<DependencyObject> BindingAction { get; set; }
    }

}
