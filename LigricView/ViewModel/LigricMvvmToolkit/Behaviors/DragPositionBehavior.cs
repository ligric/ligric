using Microsoft.Xaml.Interactivity;
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

        private UIElement parent = null;



        private Point prevPoint;
        private int pointerId = -1;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DragPositionBehavior), new PropertyMetadata(null));
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }


        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof(ZoomFactor), typeof(double), typeof(DragPositionBehavior), new PropertyMetadata(0));

        public double ZoomFactor { get => (double)GetValue(ZoomFactorProperty); set => SetValue(ZoomFactorProperty, value); }



        #region Life circle
        public void Attach(DependencyObject associatedObject)
        {
            if ((associatedObject != AssociatedObject) && !Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                AssociatedObject = associatedObject;
                var element = AssociatedObject as FrameworkElement;
                if (element != null)
                {
                    element.PointerPressed += OnElementPointerPressed;
                    element.PointerReleased += OnElementPointerReleased;
                }
            }
        }

        public void Detach()
        {
            var element = AssociatedObject as FrameworkElement;

            if (element is not null)
            {
                element.PointerPressed -= OnElementPointerPressed;
                element.PointerReleased -= OnElementPointerReleased;
            }

            if (parent is not null)
            {
                parent.PointerMoved -= OnMove;
            }

            parent = null;
            AssociatedObject = null;
        }
        #endregion

        #region Handle pointer input
        private void OnElementPointerPressed(object sender, PointerRoutedEventArgs e)
        {
#if NET6_0_ANDROID
            //NativeScrollContentPresenter fassd;
            //Windows.UI.Xaml.Controls.NativeScrollContentPresenter ffffffff;


            var element = AssociatedObject as FrameworkElement;

            var parentAsUIElement = sender as UIElement;

            if (parentAsUIElement == null)
                return;

            parent = (UIElement)parentAsUIElement.Parent;


            Android.Views.IViewParent? das =  parentAsUIElement.Parent;


#else

            var element = AssociatedObject as FrameworkElement;

            if (element == null)
                return;

            parent = (UIElement)element.Parent;
#endif


            if (!(element.RenderTransform is TranslateTransform))
                element.RenderTransform = new TranslateTransform();

            prevPoint = e.GetCurrentPoint(parent).Position;
            parent.PointerMoved += OnMove;
            pointerId = (int)e.Pointer.PointerId;
        }
        
        private void OnElementPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId != pointerId)
                return;

            parent.PointerMoved -= OnMove;
            pointerId = -1;


            Point position = e.GetCurrentPoint(parent).Position;
            if (Command.CanExecute(position))
            {
                Command.Execute(position);
            }
        }

        private void OnMove(object o, PointerRoutedEventArgs args)
        {
            var zommFactor = ZoomFactor;

            if (args.Pointer.PointerId != pointerId)
                return;

            var element = AssociatedObject as FrameworkElement;

            if (element is null)
                return;

            var pos = args.GetCurrentPoint(parent).Position;
            ((TranslateTransform)element.RenderTransform).X += (pos.X - prevPoint.X) / zommFactor;
            ((TranslateTransform)element.RenderTransform).Y += (pos.Y - prevPoint.Y) / zommFactor;
            prevPoint = pos;
        }
        #endregion
    }
}
