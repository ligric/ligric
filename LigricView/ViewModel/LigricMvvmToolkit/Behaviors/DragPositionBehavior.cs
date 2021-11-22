using Common;
using Microsoft.Xaml.Interactivity;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace LigricMvvmToolkit.Behaviors
{
    public partial class DragPositionBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject{ get; set; }

        private Point prevPoint;
        private int pointerId = -1;


        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DragPositionBehavior), new PropertyMetadata(null));
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }


        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof(ZoomFactor), typeof(double), typeof(DragPositionBehavior), new PropertyMetadata(0));
        public double ZoomFactor { get => (double)GetValue(ZoomFactorProperty); set => SetValue(ZoomFactorProperty, value); }

       
        public static readonly DependencyProperty BaseElementProperty = DependencyProperty.Register(nameof(BaseElement), typeof(UIElement), typeof(DragPositionBehavior), new PropertyMetadata(null));
        public UIElement BaseElement { get => (UIElement)GetValue(BaseElementProperty); set => SetValue(BaseElementProperty, value); }


        #region Life circle
        public void Attach(DependencyObject associatedObject)
        {
            if ((associatedObject != AssociatedObject) && !Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                AssociatedObject = associatedObject;
                
                var element = AssociatedObject as FrameworkElement;
                if (element != null)
                {
                    BaseElement.PointerPressed += OnElementPointerPressed;
                    BaseElement.PointerReleased += OnElementPointerReleased;
                }
            }
        }

        public void Detach()
        {
            var element = AssociatedObject as FrameworkElement;

            if (BaseElement is not null)
            {
                BaseElement.PointerPressed -= OnElementPointerPressed;
                BaseElement.PointerReleased -= OnElementPointerReleased;
                BaseElement.PointerMoved -= OnMove;
            }

            BaseElement = null;
            AssociatedObject = null;
        }
        #endregion

        #region Handle pointer input
        private void OnElementPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var element = AssociatedObject as FrameworkElement;

            if (element == null)
                return;

            if (!(element.RenderTransform is TranslateTransform))
                element.RenderTransform = new TranslateTransform();
            
            BaseElement.PointerMoved += OnMove;

            prevPoint = e.GetCurrentPoint(BaseElement).Position;
            pointerId = (int)e.Pointer.PointerId;
        }

        private void OnElementPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId != pointerId)
                return;

            var element = AssociatedObject as FrameworkElement;
            if (element == null)
                return;

            BaseElement.PointerMoved -= OnMove;
            pointerId = -1;

            Point position = e.GetCurrentPoint(element).Position;

            if (Command.CanExecute(position))
                Command.Execute(position);
        }

        private void OnMove(object o, PointerRoutedEventArgs e)
        {
            var zommFactor = ZoomFactor;
            var element = AssociatedObject as FrameworkElement;

            if (e.Pointer.PointerId != pointerId || element is null)
                return;

            var pos = e.GetCurrentPoint(BaseElement).Position;
            ((TranslateTransform)element.RenderTransform).X += (pos.X - prevPoint.X) / zommFactor;
            ((TranslateTransform)element.RenderTransform).Y += (pos.Y - prevPoint.Y) / zommFactor;
            prevPoint = pos;
        }
        #endregion
    }
}
