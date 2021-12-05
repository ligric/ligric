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
        private bool isLoaded;

        protected readonly string c_sliderBackgroundBorder = "SliderBackgroundBorder";
        protected readonly string c_expanderHeader = "ExpanderHeader";       
        protected readonly string c_expanderContent = "ExpanderContent";

        private FrameworkElement sliderBackgroundBorder;
        private FrameworkElement expanderHeader;
        private FrameworkElement expanderContent;

        public static DependencyProperty MainParentProperty { get; } = DependencyProperty.Register("MainParent", typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null));
        public FrameworkElement MainParent
        {
            get { return (FrameworkElement)GetValue(MainParentProperty); }
            set { SetValue(MainParentProperty, value); }
        }


        public static DependencyProperty HeaderBufferProperty { get; } = DependencyProperty.Register("HeaderBuffer", typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null, OnHeaderBufferChanged));

        private static void OnHeaderBufferChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newBuffer = e.NewValue as FrameworkElement;
            if (newBuffer is null)
                return;

            var thisObject = (MenuStandard)d;

            if (!thisObject.IsLoaded)
                return;

            BufferForm(thisObject, newBuffer);
        }

        private static void BufferForm(MenuStandard thisObject, FrameworkElement buffer)
        {
            var elementVisualRelative = buffer.TransformToVisual(thisObject.MainParent);
            Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).X = bufferPostition.X;
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).Y = bufferPostition.Y;

            thisObject.expanderHeader.Width = buffer.ActualWidth;
            thisObject.expanderHeader.Height = buffer.ActualHeight;
        }

        public FrameworkElement HeaderBuffer
        {
            get { return (FrameworkElement)GetValue(HeaderBufferProperty); }
            set { SetValue(HeaderBufferProperty, value); }
        }


        public MenuStandard() : base() 
        {
            this.Loaded += OnMenuStandardLoaded;

            this.Collapsed += OnMenuStandardCollapsed;
            this.Expanding += OnMenuStandardExpanding;
        }

        #region Initialization
        private void InitializeState()
        {
            BufferForm(this, HeaderBuffer);

            if (this.IsExpanded)
            {
                ExpanderExpanding();
            }
            else
            {
                ExpanderCollapsing();
            }
        }

        private bool TransformInitialize(FrameworkElement element)
        {
            if (element is null)
                return false;

            var renderTransform = element.RenderTransform as TranslateTransform;

            if (renderTransform is null)
            {
                renderTransform = new TranslateTransform();
                element.RenderTransform = renderTransform;
            }
            return true;
        }
        #endregion

        #region Expander actions
        private void OnMenuStandardLoaded(object sender, RoutedEventArgs e)
        {
            sliderBackgroundBorder = GetTemplateChild(c_sliderBackgroundBorder) as FrameworkElement;
            expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;
            expanderContent = GetTemplateChild(c_expanderContent) as FrameworkElement;

            TransformInitialize(sliderBackgroundBorder);
            TransformInitialize(expanderHeader);
            TransformInitialize(expanderContent);

            isLoaded = true;
            InitializeState();
        }

        private void OnMenuStandardCollapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderCollapsing();
        }

        private void OnMenuStandardExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderExpanding();
        }
        #endregion
    
        #region Set expander animation
        private void ExpanderCollapsing()
        {
            justStoryboard.Stop();
            justStoryboard = new Storyboard();
            SliderAnimationCollapsed(TimeSpan.FromMilliseconds(400));
            ExpanderContentAnimationCollapsed(TimeSpan.FromMilliseconds(400));
            justStoryboard.Begin();
        }

        private void ExpanderExpanding()
        {
            justStoryboard.Stop();
            justStoryboard = new Storyboard();
            SliderAnimationExpanding(TimeSpan.FromMilliseconds(400));
            ExpanderContentAnimationExpanding(TimeSpan.FromMilliseconds(400));
            justStoryboard.Begin();
        }
        #endregion

        #region Animations
        private void SliderAnimationCollapsed(TimeSpan timeSpan)
        {
            if (!TransformInitialize(sliderBackgroundBorder) && isLoaded)
                return;

            var duration = new Duration(timeSpan);

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            sliderBackgroundBorder.Visibility = Visibility.Visible;

            #region widthAnimation
            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualWidth,
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
                From = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualHeight,
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
        }

        private void SliderAnimationExpanding(TimeSpan timeSpan)
        {
            if (!TransformInitialize(sliderBackgroundBorder) && isLoaded)
                return;

            var duration = new Duration(timeSpan);

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            sliderBackgroundBorder.Visibility = Visibility.Visible;

            #region widthAnimation
            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = expanderHeader.ActualWidth,
                To = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualWidth,
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
                To = ((MainParent is null ? (FrameworkElement)this.Parent : MainParent)).ActualHeight,
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
        }

        private void ExpanderContentAnimationExpanding(TimeSpan timeSpan)
        {
            if (!TransformInitialize(expanderContent) && isLoaded)
                return;

            var duration = new Duration(timeSpan);
            var renderTransform = expanderContent.RenderTransform as TranslateTransform;

            expanderContent.Visibility = Visibility.Visible;

            #region yAnimation
            DoubleAnimation yAnimationKek = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = -300,
                To = 0,
                Duration = duration
            };
            Storyboard.SetTarget(yAnimationKek, renderTransform);
            Storyboard.SetTargetProperty(yAnimationKek, "Y");
            #endregion

            justStoryboard.Children.Add(yAnimationKek);
        }

        private void ExpanderContentAnimationCollapsed(TimeSpan timeSpan)
        {
            if (!TransformInitialize(expanderContent) && isLoaded)
                return;

            var duration = new Duration(timeSpan);
            var renderTransform = expanderContent.RenderTransform as TranslateTransform;

            expanderContent.Visibility = Visibility.Visible;

            #region yAnimation
            DoubleAnimation yAnimationKek = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = -300,
                Duration = duration
            };
            Storyboard.SetTarget(yAnimationKek, renderTransform);
            Storyboard.SetTargetProperty(yAnimationKek, "Y");
            #endregion

            #region visibilityAnimation
            ObjectAnimationUsingKeyFrames visibilityAnimation = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame visibilityCollapsedKeyFrame = new DiscreteObjectKeyFrame()
            {
                KeyTime = timeSpan,
                Value = Visibility.Collapsed
            };
            visibilityAnimation.KeyFrames.Add(visibilityCollapsedKeyFrame);

            Storyboard.SetTarget(visibilityAnimation, expanderContent);
            Storyboard.SetTargetProperty(visibilityAnimation, "Visibility");
            #endregion

            justStoryboard.Children.Add(yAnimationKek);
            justStoryboard.Children.Add(visibilityAnimation);
        }
        #endregion
    }
}
