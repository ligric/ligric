using Microsoft.UI.Xaml.Controls;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.Menus
{
    public partial class MenuStandard : Expander
    {
        private readonly string c_sliderBackgroundBorder = "SliderBackgroundBorder";
        private readonly string c_expanderHeader = "ExpanderHeader";

        public FrameworkElement ParentElement { get => (FrameworkElement)GetValue(ParentElementProperty); set => SetValue(ParentElementProperty, value); }
        public static readonly DependencyProperty ParentElementProperty = DependencyProperty.Register(nameof(ParentElement), typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null));

        public Rect Rect { get => (Rect)GetValue(RectProperty); set => SetValue(RectProperty, value); }
        public static readonly DependencyProperty RectProperty = DependencyProperty.Register(nameof(Rect), typeof(Rect), typeof(MenuStandard), new PropertyMetadata(default(Rect)));
        

        public MenuStandard() : base() 
        {
            this.Collapsed += OnMenuStandardCollapsed;
            this.Expanding += OnMenuStandardExpanding;

            //this.TransformMatrix  TransformToVisual

            //((Grid)this.Header).ActualOffset

            Task.Run(async () =>
            {
                await Task.Delay(2_000);

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    var test = this;
                }
                );
            }
            );
        }

        private void OnMenuStandardCollapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            SliderAnimationCollapsed(TimeSpan.FromMilliseconds(500));
        }

        private void OnMenuStandardExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            SliderAnimationExpanding(TimeSpan.FromMilliseconds(500));
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

            Storyboard justintimeStoryboard = new Storyboard();
            justintimeStoryboard.Children.Add(widthAnimation);
            justintimeStoryboard.Children.Add(heightAnimation);
            justintimeStoryboard.Children.Add(xAnimation);
            justintimeStoryboard.Children.Add(yAnimation);
            justintimeStoryboard.Children.Add(visibilityAnimation);
            justintimeStoryboard.Begin();
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
                To = this.ActualWidth,
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
                To = this.ActualHeight,
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

            Storyboard justintimeStoryboard = new Storyboard();
            justintimeStoryboard.Children.Add(widthAnimation);
            justintimeStoryboard.Children.Add(heightAnimation);
            justintimeStoryboard.Children.Add(xAnimation);
            justintimeStoryboard.Children.Add(yAnimation);
            justintimeStoryboard.Begin();
        }
    }
}
