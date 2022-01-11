﻿using Microsoft.UI.Xaml.Controls;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.Menus
{
    public partial class PourMenu : Expander
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


            var compositor = Window.Current.Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(thisObject.sliderBackgroundBorder);

            var actualWidth = (thisObject.MainParent is null ? (FrameworkElement)thisObject.Parent : thisObject.MainParent).ActualWidth;

            var geometry = compositor.CreateRectangleGeometry();
            geometry.Offset = new System.Numerics.Vector2((float)bufferPostition.X, (float)bufferPostition.Y);
            geometry.Size = new System.Numerics.Vector2((float)actualWidth - 20, (float)thisObject.MainParent.ActualHeight);
            var clip = compositor.CreateGeometricClip(geometry);

            visual.Clip = clip;
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
            sliderBackgroundBorder.Visibility = Visibility.Collapsed;

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
        private void OnPourMenuLoaded(object s, RoutedEventArgs e)
        {
            this.Collapsed += OnPourMenuCollapsed;
            this.Expanding += OnPourMenuExpanding;

            sliderBackgroundBorder = GetTemplateChild(c_sliderBackgroundBorder) as FrameworkElement;
            expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;
            expanderContent = GetTemplateChild(c_expanderContent) as FrameworkElement;

            TransformInitialize(sliderBackgroundBorder);
            TransformInitialize(expanderHeader);
            TransformInitialize(expanderContent);

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
        private void ExpanderCollapsing()
        {
            //var geometry = compositor.CreateRectangleGeometry();
            //geometry. = new System.Numerics.Vector2(200, 200);
            //geometry.Radius = Vector2.Zero;
            //var clip = compositor.CreateGeometricClip(geometry);



            //var animation = compositor.CreateVector2KeyFrameAnimation();

            //animation.DelayTime = TimeSpan.FromMilliseconds(10_000);
            //animation.Duration = TimeSpan.FromMilliseconds(5_000);
            //animation.InsertKeyFrame(1, new Vector2(50, 100));

            //geometry.StartAnimation(nameof(CompositionEllipseGeometry.Radius), animation);

            justStoryboard.Pause();
            justStoryboard = new Storyboard();

            SliderAnimationCollapsing(TimeSpan.FromMilliseconds(300));
            ExpanderContentAnimationCollapsed(TimeSpan.FromMilliseconds(200));

            justStoryboard.Begin();

            justStoryboard.Completed += (s, e) =>
            {
                sliderBackgroundBorder.Visibility = Visibility.Collapsed;
                expanderContent.Visibility = Visibility.Collapsed;
            };
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

            justStoryboard.Children.Add(yAnimation);
        }
        #endregion
    }
}
