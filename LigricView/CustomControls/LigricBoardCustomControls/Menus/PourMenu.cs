using Common.Executions;
using LigricMvvmToolkit.Extensions;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.Menus
{
    public partial class PourMenu : Expander
    {
        private Storyboard justStoryboard = new Storyboard();
        private bool isLoaded;

        protected readonly string c_sliderBackgroundBorder = "SliderBackgroundBorder";
        protected readonly string c_expanderContentClip = "ExpanderContentClip";
        protected readonly string c_expanderHeader = "ExpanderHeader";
        protected readonly string c_expanderContent = "ExpanderContent";

        private FrameworkElement sliderBackgroundBorder;
        private FrameworkElement expanderContentClip;
        private FrameworkElement expanderHeader;
        private FrameworkElement expanderContent;

        private enum HeaderSideEnum
        {
            Left,
            Right,
        }

        private HeaderSideEnum headerSide;

        public static DependencyProperty MainParentProperty { get; } = DependencyProperty.Register("MainParent", typeof(FrameworkElement), typeof(PourMenu), new PropertyMetadata(null, OnMainParentChanged));

        private static void OnMainParentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mainParent = e.NewValue as FrameworkElement;
            if (mainParent is null)
                return;

            var thisObject = (PourMenu)d;

            BufferUpdate(thisObject);

            mainParent.SizeChanged += (sender, eventArgs) => BufferUpdate(thisObject);
        }

        public FrameworkElement MainParent
        {
            get { return (FrameworkElement)GetValue(MainParentProperty); }
            set { SetValue(MainParentProperty, value); }
        }


        public static DependencyProperty HeaderBufferProperty { get; } = DependencyProperty.Register("HeaderBuffer", typeof(FrameworkElement), typeof(PourMenu), new PropertyMetadata(null, OnHeaderBufferChanged));
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

            var thisObject = (PourMenu)d;

            //BufferForm(thisObject, newBuffer);
            //newBuffer.SizeChanged += (sender, eventArgs) => BufferForm(thisObject, newBuffer);
        }

        private static void BufferUpdate(PourMenu thisObject)
        {
            if (!thisObject.IsLoaded)
                return;

            ////// Set start Size
            thisObject.expanderHeader.Width = thisObject.HeaderBuffer.ActualWidth;
            thisObject.expanderHeader.Height = thisObject.HeaderBuffer.ActualHeight;

            ////// Set start Postion
            var elementVisualRelative = thisObject.HeaderBuffer.TransformToVisual(thisObject.MainParent);
            Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).X = bufferPostition.X;
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).Y = bufferPostition.Y;

            var actualWidth = (thisObject.MainParent is null ? (FrameworkElement)thisObject.Parent : thisObject.MainParent).ActualWidth;
            var actualHeight = (thisObject.MainParent is null ? (FrameworkElement)thisObject.Parent : thisObject.MainParent).ActualHeight;
            thisObject.expanderContentClip.Clip = new RectangleGeometry()
            {
                Rect = new Rect(new Point(bufferPostition.X, bufferPostition.Y),
                                new Point(actualWidth - bufferPostition.X, actualHeight - bufferPostition.Y))
            };
        }

        public PourMenu() : base()
        {
            this.DefaultStyleKey = typeof(PourMenu);

            this.Loaded += OnPourMenuLoaded;
        }

        #region Initialization
        private void InitializeState()
        {
            BufferUpdate(this);

            isLoaded = true;

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
            var actualWidth = (this.MainParent is null ? (FrameworkElement)this.Parent : this.MainParent).ActualWidth;
            var actualHeight = (this.MainParent is null ? (FrameworkElement)this.Parent : this.MainParent).ActualHeight;

            // Board
            sliderBackgroundBorder.Visibility = Visibility.Visible;

            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).X = 0;
            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).Y = 0;

            sliderBackgroundBorder.Width = actualWidth;
            sliderBackgroundBorder.Height = actualHeight;


            // Content clip
            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));
            expanderContentClip.Clip = new RectangleGeometry()
            {
                Rect = new Rect(new Point(headerPostition.X, headerPostition.Y),
                                new Point(actualWidth - headerPostition.X, actualHeight - headerPostition.Y))
            };

            // Content
            expanderContent.Visibility = Visibility.Visible;
            ((TranslateTransform)expanderContent.RenderTransform).Y = 0;
        }

        private void ExpanderInitializeCollapsing()
        {
            var elementVisualRelative = expanderHeader.TransformToVisual(this);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            // Board
            sliderBackgroundBorder.Visibility = Visibility.Collapsed;


            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).X = headerPostition.X;
            ((TranslateTransform)sliderBackgroundBorder.RenderTransform).Y = headerPostition.Y;

            sliderBackgroundBorder.Height = expanderHeader.ActualHeight;
            sliderBackgroundBorder.Width = expanderHeader.ActualWidth;


            // Content clip
            var actualWidth = (this.MainParent is null ? (FrameworkElement)this.Parent : this.MainParent).ActualWidth;
            var actualHeight = (this.MainParent is null ? (FrameworkElement)this.Parent : this.MainParent).ActualHeight;
            expanderContentClip.Clip = new RectangleGeometry()
            {
                Rect = new Rect(new Point(headerPostition.X, headerPostition.Y),
                                new Point(actualWidth - headerPostition.X, actualHeight - headerPostition.Y))
            };

            // Content
            var renderTransform = expanderContent.RenderTransform as TranslateTransform;
            double to = (MainParent is null ? (FrameworkElement)this.Parent : MainParent).ActualHeight - renderTransform.Y;

            ((TranslateTransform)expanderContent.RenderTransform).Y = -to;
            expanderContent.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Expander actions
        private void OnPourMenuLoaded(object s, RoutedEventArgs e)
        {
            this.Collapsed += OnPourMenuCollapsed;
            this.Expanding += OnPourMenuExpanding;

            sliderBackgroundBorder = GetTemplateChild(c_sliderBackgroundBorder) as FrameworkElement;
            expanderContentClip = GetTemplateChild(c_expanderContentClip) as FrameworkElement;
            expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;
            expanderContent = GetTemplateChild(c_expanderContent) as FrameworkElement;

            sliderBackgroundBorder.TransformInitialize();
            expanderContentClip.TransformInitialize();
            expanderHeader.TransformInitialize();
            expanderContent.TransformInitialize();

            InitializeState();
        }

        private void OnPourMenuCollapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderCollapsing();
        }

        private void OnPourMenuExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderExpanding();
        }
        #endregion

        #region Set expander animation
        private readonly CancelableExecution sliderAnimationTokenSource = new CancelableExecution();
        //private Task sliderSecondWidthAnimationTask;

        private void ExpanderCollapsing()
        {
            //sliderAnimationTokenSource.Cancel();
            justStoryboard.Pause();
            justStoryboard = new Storyboard();

            double timeMilliseconds = 300;

            SliderAnimationCollapsing(TimeSpan.FromMilliseconds(timeMilliseconds));
            ExpanderContentAnimationCollapsed(TimeSpan.FromMilliseconds(timeMilliseconds / 1.5));           

            justStoryboard.Completed += async (s, e) =>
            {
                await Task.Run(async () =>
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        justStoryboard.Pause();
                        justStoryboard = new Storyboard();

                        #region widthAnimation
                        DoubleAnimationUsingKeyFrames widthAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
                        var halfTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(timeMilliseconds / 2));
                        widthAnimation.KeyFrames.Add(new SplineDoubleKeyFrame() { Value = expanderHeader.ActualWidth, KeyTime = halfTime });
                        Storyboard.SetTarget(widthAnimation, sliderBackgroundBorder);
                        Storyboard.SetTargetProperty(widthAnimation, "Width");
                        #endregion

                        justStoryboard.Children.Add(widthAnimation);


                        justStoryboard.Completed += (ss, ee) =>
                        {
                            sliderBackgroundBorder.Visibility = Visibility.Collapsed;
                            expanderContent.Visibility = Visibility.Collapsed;
                        };

                        justStoryboard.Begin();
                    });
                });
            };
            justStoryboard.Begin();
        }

        private void ExpanderExpanding()
        {
            justStoryboard.Pause();
            justStoryboard = new Storyboard();

            SliderAnimationExpanding(TimeSpan.FromMilliseconds(150));
            ExpanderContentAnimationExpanding(TimeSpan.FromMilliseconds(250));

            justStoryboard.Begin();
        }
        #endregion

        #region Animations
        private void SliderAnimationExpanding(TimeSpan timeSpan)
        {
            if (!sliderBackgroundBorder.TransformInitialize() && isLoaded)
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
            if (!expanderContent.TransformInitialize() && isLoaded)
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
            if (!sliderBackgroundBorder.TransformInitialize() && isLoaded)
                return;

            var actualWidth = (this.MainParent is null ? (FrameworkElement)this.Parent : this.MainParent).ActualWidth;

            var renderTransform = sliderBackgroundBorder.RenderTransform as TranslateTransform;

            var elementVisualRelative = expanderHeader.TransformToVisual(this.MainParent is null ? (FrameworkElement)this.Parent : this.MainParent);
            Point headerPostition = elementVisualRelative.TransformPoint(new Point(0, 0));

            #region xAnimation
            DoubleAnimationUsingKeyFrames xAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            xAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = headerPostition.X, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion

            #region yAnimation
            DoubleAnimationUsingKeyFrames yAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            yAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = headerPostition.Y, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            #region heightAnimation
            DoubleAnimationUsingKeyFrames heightAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            heightAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = expanderHeader.ActualHeight, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(heightAnimation, sliderBackgroundBorder);
            Storyboard.SetTargetProperty(heightAnimation, "Height");
            #endregion

            #region widthAnimation
            DoubleAnimationUsingKeyFrames widthAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            widthAnimation.KeyFrames.Add(new SplineDoubleKeyFrame() { Value = actualWidth - 20, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
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
            if (!expanderContent.TransformInitialize() && isLoaded)
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

            justStoryboard.Children.Add(yAnimation);
        }
        #endregion
    }
}
