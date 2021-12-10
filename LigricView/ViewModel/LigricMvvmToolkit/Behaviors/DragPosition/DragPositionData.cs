using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.Behaviors.DragPosition
{
    public class DragPositionData
    {
        private object _zoomFactor;
        private object _baseParent;
        private object _offsetX;
        private object _offsetY;

        public object ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                if ((value ?? 0.0) is double ||
                    value is BindingBase ||
                    (double.TryParse(value.ToString(), out double val) && (value = val) != null))
                    _zoomFactor = value;
                else
                    throw new ArgumentException(nameof(value));
            }
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
                if ((value ?? 0.0) is double ||
                    value is BindingBase ||
                    (double.TryParse(value.ToString(), out double val) && (value = val) != null))
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
                if ((value ?? 0.0) is double ||
                    value is BindingBase ||
                    (double.TryParse(value.ToString(), out double val) && (value = val) != null))
                    _offsetY = value;
                else
                    throw new ArgumentException(nameof(value));
            }
        }

        public Action<DependencyObject> BindingAction { get; set; }
    }
}
