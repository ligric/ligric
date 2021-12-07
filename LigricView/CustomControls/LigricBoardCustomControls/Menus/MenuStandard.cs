using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        private enum HeaderSideEnum
        {
            Left,
            Right,
        }

        private HeaderSideEnum headerSide;

        public static DependencyProperty MainParentProperty { get; } = DependencyProperty.Register("MainParent", typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null));
        public FrameworkElement MainParent
        {
            get { return (FrameworkElement)GetValue(MainParentProperty); }
            set { SetValue(MainParentProperty, value); }
        }


        public static DependencyProperty HeaderBufferProperty { get; } = DependencyProperty.Register("HeaderBuffer", typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null, OnHeaderBufferChanged));
        public FrameworkElement HeaderBuffer
        {
            get { return (FrameworkElement)GetValue(HeaderBufferProperty); }
            set { SetValue(HeaderBufferProperty, value); }
        }

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

        public MenuStandard() : base()
        {
            this.DefaultStyleKey = typeof(MenuStandard);

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
                ExpanderInitializeExpanding();
            }
            else
            {
                ExpanderInitializeCollapsing();
            }
        }

        private void ExpanderInitializeExpanding()
        {
            // Board
            sliderBackgroundBorder.Visibility = Visibility.Visible;

            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).X = 0;
            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).Y = 0;

            sliderBackgroundBorder.Height = ((MainParent is null ? (FrameworkElement)this.Parent : MainParent)).ActualHeight;
            sliderBackgroundBorder.Width = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualWidth;


            // Content
            expanderContent.Visibility = Visibility.Visible;
            ((TranslateTransform)expanderContent.RenderTransform).Y = 0;
        }

        private void ExpanderInitializeCollapsing()
        {
            // Board
            sliderBackgroundBorder.Visibility = Visibility.Visible;

            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));


            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).X = headerPostition.X;
            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).Y = headerPostition.Y;

            sliderBackgroundBorder.Height = expanderHeader.ActualHeight;
            sliderBackgroundBorder.Width = expanderHeader.ActualWidth;


            // Content
            var renderTransform = expanderContent.RenderTransform as TranslateTransform;
            double to = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualHeight - renderTransform.Y;

            ((TranslateTransform)expanderContent.RenderTransform).Y = -to;
            expanderContent.Visibility = Visibility.Collapsed;
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
            justStoryboard = new Storyboard();

            SliderAnimationCollapsing(TimeSpan.FromMilliseconds(800));
            ExpanderContentAnimationCollapsed(TimeSpan.FromMilliseconds(200));

            justStoryboard.Begin();
        }

        private void ExpanderExpanding()
        {
            justStoryboard = new Storyboard();

            SliderAnimationExpanding(TimeSpan.FromMilliseconds(200));
            ExpanderContentAnimationExpanding(TimeSpan.FromMilliseconds(700));

            justStoryboard.Begin();
        }
        #endregion

        #region Animations
        private void SliderAnimationExpanding(TimeSpan timeSpan)
        {
            if (!TransformInitialize(sliderBackgroundBorder) && isLoaded)
                return;

            TimeSpan part = TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds / 2);
            TimeSpan partDouble = TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds / 4);

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            sliderBackgroundBorder.Visibility = Visibility.Visible;

            #region widthAnimation
            DoubleAnimationUsingKeyFrames widthAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            widthAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualWidth, KeyTime = KeyTime.FromTimeSpan(partDouble) });
            Storyboard.SetTarget(widthAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(widthAnimation, "Width");
            #endregion

            #region heightAnimation
            DoubleAnimationUsingKeyFrames heightAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            heightAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = ((MainParent is null ? (FrameworkElement)this.Parent : MainParent)).ActualHeight, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(heightAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(heightAnimation, "Height");
            #endregion

            #region xAnimation
            DoubleAnimationUsingKeyFrames xAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            xAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = 0, KeyTime = KeyTime.FromTimeSpan(part) });
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion

            #region yAnimation
            DoubleAnimationUsingKeyFrames yAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            yAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = 0, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
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

            var renderTransform = expanderContent.RenderTransform as TranslateTransform;

            expanderContent.Visibility = Visibility.Visible;

            #region yAnimation
            DoubleAnimationUsingKeyFrames yAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            yAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = 0, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            justStoryboard.Children.Add(yAnimation);
        }

        private void SliderAnimationCollapsing(TimeSpan timeSpan)
        {
            if (!TransformInitialize(sliderBackgroundBorder) && isLoaded)
                return;

            TimeSpan forFirstPart = TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds / 2);

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            #region xAnimation
            DoubleAnimationUsingKeyFrames xAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            xAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = headerPostition.X, KeyTime = KeyTime.FromTimeSpan(forFirstPart) });
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion

            #region yAnimation
            DoubleAnimationUsingKeyFrames yAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            yAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = headerPostition.Y, KeyTime = KeyTime.FromTimeSpan(forFirstPart) });
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            #region heightAnimation
            DoubleAnimationUsingKeyFrames heightAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            heightAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = expanderHeader.ActualHeight, KeyTime = KeyTime.FromTimeSpan(forFirstPart) });
            Storyboard.SetTarget(heightAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(heightAnimation, "Height");
            #endregion

            #region widthAnimation
            DoubleAnimationUsingKeyFrames widthAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };

            widthAnimation.KeyFrames.Add(
                new SplineDoubleKeyFrame() { Value = expanderHeader.ActualWidth, KeyTime = KeyTime.FromTimeSpan(timeSpan), KeySpline = new KeySpline() { ControlPoint1 = new Point(0.5, 0.5), ControlPoint2 = new Point(0.5, 0.0) } });

            Storyboard.SetTarget(widthAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(widthAnimation, "Width");
            #endregion

            justStoryboard.Children.Add(widthAnimation);
            justStoryboard.Children.Add(xAnimation);
            justStoryboard.Children.Add(yAnimation);
            justStoryboard.Children.Add(heightAnimation);
        }
        private void ExpanderContentAnimationCollapsed(TimeSpan timeSpan)
        {
            if (!TransformInitialize(expanderContent) && isLoaded)
                return;

            var renderTransform = expanderContent.RenderTransform as TranslateTransform;

            expanderContent.Visibility = Visibility.Visible;

            double to = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualHeight - renderTransform.Y;

            #region yAnimation
            DoubleAnimationUsingKeyFrames yAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            yAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = - to, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            #region visibilityAnimation
            ObjectAnimationUsingKeyFrames visibilityAnimation = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame visibilityCollapsedKeyFrame = new DiscreteObjectKeyFrame() { KeyTime = timeSpan, Value = Visibility.Collapsed };
            visibilityAnimation.KeyFrames.Add(visibilityCollapsedKeyFrame);
            Storyboard.SetTarget(visibilityAnimation, expanderContent);
            Storyboard.SetTargetProperty(visibilityAnimation, "Visibility");
            #endregion

            justStoryboard.Children.Add(yAnimation);
            justStoryboard.Children.Add(visibilityAnimation);
        }
        #endregion
    }
}
