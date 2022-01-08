using LigricMvvmToolkit.Data;
using LigricMvvmToolkit.Extensions;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace DragPosition
{
    public partial class DragPosition
    {
        private class HandlersData : IDisposable
        {
            public void Dispose()
            {
                IsDispose = true;
                element.RemoveHandler(UIElement.PointerPressedEvent, (PointerEventHandler)OnElementPointerPressed);
                UIElement top = element.GetTopUIElement();
                top.RemoveHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)OnElementPointerReleased);
                top.PointerMoved -= OnMove;
            }

            public bool IsDispose { get; private set; }

            private Point prevPoint;
            private int pointerId = -1;
            int countMove;

            private readonly UIElement element;

            public HandlersData(UIElement element)
            {
                this.element = element ?? throw new ArgumentNullException(nameof(element));

                element.AddHandler(UIElement.PointerPressedEvent, (PointerEventHandler)OnElementPointerPressed, true);
            }

            private void OnElementPointerPressed(object sender, PointerRoutedEventArgs e)
            {

                Debug.WriteLine($"OnElementPointerPressed sender: {sender}");

                UIElement top = element.GetTopUIElement();

                top.AddHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)OnElementPointerReleased, true);

                countMove = 0;
                top.PointerMoved += OnMove;

                prevPoint = e.GetCurrentPoint(top).Position;
                pointerId = (int)e.Pointer.PointerId;
            }

            private void OnElementPointerReleased(object sender, PointerRoutedEventArgs e)
            {
                Debug.WriteLine($"OnElementPointerReleased sender: {sender}");

                UIElement top = element.GetTopUIElement();
                top.RemoveHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)OnElementPointerReleased);

                top.PointerMoved -= OnMove;

                if (e.Pointer.PointerId != pointerId)
                    return;

                pointerId = -1;
            }

            private void OnMove(object sender, PointerRoutedEventArgs e)
            {
                Debug.WriteLine($"{countMove++}: {sender}");
                double zommFactor = 1;

                var pos = e.GetCurrentPoint((UIElement)sender).Position;

                Canvas.SetOffsetX(element, Canvas.GetOffsetX(element) + (pos.X - prevPoint.X) / zommFactor);
                Canvas.SetOffsetY(element, Canvas.GetOffsetY(element) + (pos.Y - prevPoint.Y) / zommFactor);

                prevPoint = pos;
            }


        }
    }
}
