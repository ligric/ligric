using LigricMvvmToolkit.Extensions;
using System;
using Windows.Foundation;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Navigation
{
    public static partial class NavigateAnimationExtensions
    {
        public static Storyboard GetTrainAnimationStrouyboard(this Panel wrapper, FrameworkElement firstVisibileElement = null, FrameworkElement endVisibleElement = null, double timeMilliseconds = 300, bool toRightSide = true)
        {
            double maxWidth = wrapper.ActualWidth == 0 ? wrapper.Width : wrapper.ActualWidth;
            var timespan = TimeSpan.FromMilliseconds(timeMilliseconds);
            Storyboard stroyboard = new Storyboard();

            if (firstVisibileElement != null)
            {
                if (firstVisibileElement.Parent is not Border firstElementParent)
                    throw new ArgumentNullException("First element parent isn't Border. Please use the PrerenderElement method.");

                var firstElementRealWidth = firstVisibileElement.GetFulllWidth(wrapper);

                maxWidth = maxWidth > firstElementRealWidth ? maxWidth : firstElementRealWidth;

                var fromRenderTransform = firstElementParent.GetTransformInitialize();

                #region xAnimation firstElement
                DoubleAnimationUsingKeyFrames xAnimationFirstElement = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
                xAnimationFirstElement.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = -(maxWidth), KeyTime = KeyTime.FromTimeSpan(timespan) });
                Storyboard.SetTarget(xAnimationFirstElement, fromRenderTransform);
                Storyboard.SetTargetProperty(xAnimationFirstElement, "X");
                #endregion
                stroyboard.Children.Add(xAnimationFirstElement);
            }

            if (endVisibleElement != null)
            {
                if (endVisibleElement.Parent is not Border secondElementParent)
                    throw new ArgumentNullException("First element parent isn't Border. Please use the PrerenderElement method.");

                var toRenderTransform = secondElementParent.GetTransformInitialize();

                endVisibleElement.Visibility = Visibility.Visible;

                #region xAnimation secondElement
                DoubleAnimation xAnimationSecondElement = new DoubleAnimation()
                {
                    EnableDependentAnimation = true,
                    From = maxWidth,
                    To = 0,
                    Duration = new Duration(timespan)
                };

                Storyboard.SetTarget(xAnimationSecondElement, toRenderTransform);
                Storyboard.SetTargetProperty(xAnimationSecondElement, "X");
                #endregion

                stroyboard.Children.Add(xAnimationSecondElement);
            }

            return stroyboard;
        }

        public static double GetFulllWidth(this FrameworkElement element, UIElement transformVisualElement)
        {
            var elementChildren = element.GetAllChildren().Where(x => x is FrameworkElement);

            double maxWidth = element.ActualWidth;

            foreach (var item in elementChildren)
            {
                var child = item as FrameworkElement;
                var elementVisualRelative = child.TransformToVisual(transformVisualElement);
                Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

                var sum = child.ActualWidth + bufferPostition.X;
                if (sum > maxWidth)
                    maxWidth = sum;
            }

            return maxWidth;
        }
    }
}
