using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.Menus
{
    public partial class MenuStandard : Expander
    {
        private Storyboard justStoryboard = new Storyboard();

        protected readonly string c_sliderBackgroundBorder = "SliderBackgroundBorder";
        protected readonly string c_expanderHeader = "ExpanderHeader";

        //public FrameworkElement ParentElement { get => (FrameworkElement)GetValue(ParentElementProperty); set => SetValue(ParentElementProperty, value); }
        //public static readonly DependencyProperty ParentElementProperty = DependencyProperty.Register(nameof(ParentElement), typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null));

        public Rect Rect { get => (Rect)GetValue(RectProperty); set => SetValue(RectProperty, value); }
        public static readonly DependencyProperty RectProperty = DependencyProperty.Register(nameof(Rect), typeof(Rect), typeof(MenuStandard), new PropertyMetadata(default(Rect)));
        

        public MenuStandard() : base() 
        {
            this.Collapsed += OnMenuStandardCollapsed;
            this.Expanding += OnMenuStandardExpanding;
        }

        private void OnMenuStandardCollapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            justStoryboard.Stop();
            justStoryboard = new Storyboard();
            SliderAnimationCollapsed(TimeSpan.FromMilliseconds(400));
        }

        private void OnMenuStandardExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            justStoryboard.Stop();
            justStoryboard = new Storyboard();
            SliderAnimationExpanding(TimeSpan.FromMilliseconds(400));
        }

        private void SliderAnimationCollapsed(TimeSpan timeSpan)
        {
            var duration = new Duration(timeSpan);

            FrameworkElement sliderBackgroundBorder = GetTemplateChild(c_sliderBackgroundBorder) as FrameworkElement;
            FrameworkElement expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            if (renderTransform == null)
            {
                renderTransform = new TranslateTransform();
                sliderBackgroundBorder.RenderTransform = renderTransform;
            }

            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            sliderBackgroundBorder.Visibility = Visibility.Visible;

            #region widthAnimation
            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = this.ActualWidth,
                To = expanderHeader.ActualWidth - 3,
                Duration = duration
            };
            Storyboard.SetTarget(widthAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(widthAnimation, "Width");
            #endregion

            #region heightAnimation
            DoubleAnimation heightAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = this.ActualHeight,
                To = expanderHeader.ActualHeight - 3,
                Duration = duration
            };
            Storyboard.SetTarget(heightAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(heightAnimation, "Height");
            #endregion

            #region xAnimation
            DoubleAnimation xAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = headerPostition.X,
                Duration = duration
            };
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion

            #region yAnimation
            DoubleAnimation yAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = headerPostition.Y,
                Duration = duration
            };
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            #region visibilityAnimation
            ObjectAnimationUsingKeyFrames visibilityAnimation = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame visibilityCollapsedKeyFrame = new DiscreteObjectKeyFrame()
            {
                KeyTime = timeSpan,
                Value = Visibility.Collapsed
            };
            visibilityAnimation.KeyFrames.Add(visibilityCollapsedKeyFrame);

            Storyboard.SetTarget(visibilityAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(visibilityAnimation, "Visibility");
            #endregion

            justStoryboard.Children.Add(widthAnimation);
            justStoryboard.Children.Add(heightAnimation);
            justStoryboard.Children.Add(xAnimation);
            justStoryboard.Children.Add(yAnimation);
            justStoryboard.Children.Add(visibilityAnimation);
            justStoryboard.Begin();
        }

        private void SliderAnimationExpanding(TimeSpan timeSpan)
        {
            var duration = new Duration(timeSpan);

            FrameworkElement sliderBackgroundBorder = GetTemplateChild(c_sliderBackgroundBorder) as FrameworkElement;
            FrameworkElement expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            if (renderTransform == null)
            {
                renderTransform = new TranslateTransform();
                sliderBackgroundBorder.RenderTransform = renderTransform;
            }

            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            sliderBackgroundBorder.Visibility = Visibility.Visible;

            #region widthAnimation
            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = expanderHeader.ActualWidth,
                To = ((FrameworkElement)this.Parent).ActualWidth,
                Duration = duration
            };
            Storyboard.SetTarget(widthAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(widthAnimation, "Width");
            #endregion

            #region heightAnimation
            DoubleAnimation heightAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = expanderHeader.ActualHeight - 3,
                To = ((FrameworkElement)this.Parent).ActualHeight,
                Duration = duration
            };

            Storyboard.SetTarget(heightAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(heightAnimation, "Height");
            #endregion

            #region xAnimation
            DoubleAnimation xAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = headerPostition.X,
                To = 0,
                Duration = duration
            };
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion

            #region yAnimation
            DoubleAnimation yAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = headerPostition.Y,
                To = 0,
                Duration = duration
            };
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            justStoryboard.Children.Add(widthAnimation);
            justStoryboard.Children.Add(heightAnimation);
            justStoryboard.Children.Add(xAnimation);
            justStoryboard.Children.Add(yAnimation);
            justStoryboard.Begin();
        }
    }
}
