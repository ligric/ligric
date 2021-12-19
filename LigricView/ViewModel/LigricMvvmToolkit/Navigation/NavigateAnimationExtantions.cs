using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Navigation
{
    public static class NavigateAnimationExtantions
    {
        public static Storyboard GetTrainAnimationStrouyboard(this FrameworkElement root, FrameworkElement firstVisibileElement, FrameworkElement endVisibleElement, double timeMilliseconds)
        {
            //firstVisibileElement.AddGridWrapper()
                                //.AddElement(endVisibleElement, out int index);

            var timespan = TimeSpan.FromMilliseconds(timeMilliseconds);
            Storyboard stroyboard = new Storyboard();
            var fromRenderTransform = TransformInitialize(firstVisibileElement);
            var toRenderTransform = TransformInitialize(endVisibleElement);

            // Set default properties
            var elementEndVisualRelative = firstVisibileElement.TransformToVisual(root);
            Point endElementStartPostition = elementEndVisualRelative.TransformPoint(new Point(0, 0));

            ((TranslateTransform)endVisibleElement.RenderTransform).X = endElementStartPostition.X;

            #region xAnimation firstElement
            DoubleAnimationUsingKeyFrames xAnimationFirstElement = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            xAnimationFirstElement.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = -firstVisibileElement.ActualWidth, KeyTime = KeyTime.FromTimeSpan(timespan) });
            Storyboard.SetTarget(xAnimationFirstElement, fromRenderTransform);
            Storyboard.SetTargetProperty(xAnimationFirstElement, "X");
            #endregion

            //#region xAnimation secondElement
            //DoubleAnimationUsingKeyFrames xAnimationSecondElement = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            //xAnimationSecondElement.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = 0, KeyTime = KeyTime.FromTimeSpan(timespan) });
            //Storyboard.SetTarget(xAnimationSecondElement, toRenderTransform);
            //Storyboard.SetTargetProperty(xAnimationSecondElement, "X");
            //#endregion

            stroyboard.Children.Add(xAnimationFirstElement);
            //stroyboard.Children.Add(xAnimationSecondElement);

            return stroyboard;
        }




        private static TranslateTransform TransformInitialize(FrameworkElement element)
        {
            if (element is null)
                return null;

            var renderTransform = element.RenderTransform as TranslateTransform;

            if (renderTransform is null)
            {
                renderTransform = new TranslateTransform();
                element.RenderTransform = renderTransform;
            }
            return renderTransform;
        }
    }
}
