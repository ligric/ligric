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
            var firstVisibileElementChildren = firstVisibileElement.GetAllChildren().Where(x => x is FrameworkElement);

            double maxWidth = 0;
            foreach (var item in firstVisibileElementChildren)
            {
                var element = item as FrameworkElement;
                var elementVisualRelative = element.TransformToVisual(wrapper);
                Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

                var sum = element.ActualWidth + bufferPostition.X;
                if (sum > maxWidth)
                    maxWidth = sum;
            }

            
            var rootWidth = wrapper.ActualWidth == 0 ? wrapper.Width : wrapper.ActualWidth;

            var mainWidth = rootWidth >= maxWidth ? rootWidth : maxWidth;


            var timespan = TimeSpan.FromMilliseconds(timeMilliseconds);
            Storyboard stroyboard = new Storyboard();

            if (firstVisibileElement != null)
            {
                var fromRenderTransform = firstVisibileElement.GetTransformInitialize();

                #region xAnimation firstElement
                DoubleAnimationUsingKeyFrames xAnimationFirstElement = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
                xAnimationFirstElement.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = -(mainWidth), KeyTime = KeyTime.FromTimeSpan(timespan) });
                Storyboard.SetTarget(xAnimationFirstElement, fromRenderTransform);
                Storyboard.SetTargetProperty(xAnimationFirstElement, "X");
                #endregion
                stroyboard.Children.Add(xAnimationFirstElement);
            }

            if (endVisibleElement != null)
            {
                var toRenderTransform = endVisibleElement.GetTransformInitialize();

                endVisibleElement.Visibility = Visibility.Visible;

                #region xAnimation secondElement
                DoubleAnimation xAnimationSecondElement = new DoubleAnimation()
                {
                    EnableDependentAnimation = true,
                    From = mainWidth,
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
    }
}
