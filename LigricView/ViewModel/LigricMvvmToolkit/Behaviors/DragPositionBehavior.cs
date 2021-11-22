using Microsoft.Xaml.Interactivity;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace LigricMvvmToolkit.Behaviors
{
    public partial class DragPositionBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; set; }
        public UIElement AssociatedUIElement { get; private set; }


        private Point prevPoint;
        private int pointerId = -1;

        public static readonly DependencyProperty TransformProperty = DependencyProperty.RegisterAttached(nameof(Transform), typeof(Transform), typeof(DragPositionBehavior), new PropertyMetadata(null));
        public Transform Transform { get => (Transform)GetValue(TransformProperty); set => SetValue(TransformProperty, value); }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DragPositionBehavior), new PropertyMetadata(null));
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }


        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register(nameof(ZoomFactor), typeof(double), typeof(DragPositionBehavior), new PropertyMetadata(0));
        public double ZoomFactor { get => (double)GetValue(ZoomFactorProperty); set => SetValue(ZoomFactorProperty, value); }


        public static readonly DependencyProperty BaseParentProperty = DependencyProperty.Register(nameof(BaseParent), typeof(UIElement), typeof(DragPositionBehavior), new PropertyMetadata(null));
        public UIElement BaseParent { get => (UIElement)GetValue(BaseParentProperty); set => SetValue(BaseParentProperty, value); }


        #region Life circle
        public void Attach(DependencyObject associatedObject)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (!(associatedObject is UIElement associatedUIElement))
            {
                throw new ArgumentException($"Type: {nameof(DragPositionBehavior)};\nMethod: {nameof(Attach)}\nProblem: \"associatedObject is not UIElement associatedUIElement\".", nameof(associatedObject));
            }

            if (associatedObject != AssociatedObject)
            {
                AssociatedObject = associatedObject;
                AssociatedUIElement = associatedUIElement;

                associatedUIElement.PointerPressed += OnElementPointerPressed;
            }
        }

        public void Detach()
        {
            AssociatedUIElement.PointerPressed -= OnElementPointerPressed;
            AssociatedObject = null;
            AssociatedUIElement = null;
        }
        #endregion

        #region Handle pointer input
        private void OnElementPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (BaseParent == null)
                return;

            BaseParent.PointerReleased += OnElementPointerReleased;

            // TODO : Дописать событие выхода за пределы панели.
            // TODO : Это очень криво. Может у элемента есть своя друга трансформация?
            // TODO : Это нужно ОБЯЗАТЕЛЬНО заменить на AP-свойства.

            if (!(AssociatedUIElement.RenderTransform is TranslateTransform))
                AssociatedUIElement.RenderTransform = new TranslateTransform();

            BaseParent.PointerMoved += OnMove;

            prevPoint = e.GetCurrentPoint(BaseParent).Position;
            pointerId = (int)e.Pointer.PointerId;
        }

        private void OnElementPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var basePanel = (UIElement)sender;
            basePanel.PointerReleased -= OnElementPointerReleased;
            basePanel.PointerMoved -= OnMove;

            if (e.Pointer.PointerId != pointerId)
                return;

            if (AssociatedUIElement == null)
                return;

            pointerId = -1;
            //Point position = e.GetCurrentPoint(element).Position;
            //if (Command.CanExecute(position))
            //    Command.Execute(position);
        }

        private void OnMove(object sender, PointerRoutedEventArgs e)
        {
            var zommFactor = ZoomFactor;

            if (AssociatedUIElement is null)
                return;

            var pos = e.GetCurrentPoint(BaseParent).Position;
            ((TranslateTransform)AssociatedUIElement.RenderTransform).X += (pos.X - prevPoint.X) / zommFactor;
            ((TranslateTransform)AssociatedUIElement.RenderTransform).Y += (pos.Y - prevPoint.Y) / zommFactor;
            prevPoint = pos;
        }
        #endregion
    }
}
