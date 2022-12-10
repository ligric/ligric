using LigricMvvmToolkit.Extensions;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.Menus
{
    public partial class ExpanderStandard : Expander
    {
        private Storyboard justStoryboard = new Storyboard();
        private bool isLoaded;

        protected readonly string c_expanderHeader = "ExpanderHeader";
        protected readonly string c_expanderContent = "ExpanderContent";

        private FrameworkElement expanderHeader;
        private FrameworkElement expanderContent;

        public ExpanderStandard() : base()
        {
            this.DefaultStyleKey = typeof(ExpanderStandard);

            this.Loaded += OnExpanderStandardLoaded;

            this.Collapsed += OnExpanderStandardCollapsed;
            this.Expanding += OnExpanderStandardExpanding;
        }

        #region Initialization
        private void InitializeState()
        {

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
            // Content
            expanderContent.Visibility = Visibility.Visible;
            ((TranslateTransform)expanderContent.RenderTransform).Y = 0;
        }

        private void ExpanderInitializeCollapsing()
        {
            // Content
            double to = expanderHeader.ActualHeight + expanderContent.ActualHeight;

            ((TranslateTransform)expanderContent.RenderTransform).Y = -to;
            expanderContent.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Expander actions
        private void OnExpanderStandardLoaded(object sender, RoutedEventArgs e)
        {
            expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;
            expanderContent = GetTemplateChild(c_expanderContent) as FrameworkElement;

            expanderHeader.TransformInitialize();
            expanderContent.TransformInitialize();

            isLoaded = true;
            InitializeState();
        }

        private void OnExpanderStandardCollapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderCollapsing();
        }

        private void OnExpanderStandardExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderExpanding();
        }
        #endregion

        #region Set expander animation
        private void ExpanderCollapsing()
        {
            justStoryboard?.Pause();
            justStoryboard = new Storyboard();

            ExpanderContentAnimationCollapsed(TimeSpan.FromMilliseconds(200));

            justStoryboard.Begin();

            justStoryboard.Completed += (s, e) =>
            {
                expanderContent.Visibility = Visibility.Collapsed;
            };
        }

        private void ExpanderExpanding()
        {
            justStoryboard?.Pause();
            justStoryboard = new Storyboard();

            ExpanderContentAnimationExpanding(TimeSpan.FromMilliseconds(250));

            justStoryboard.Begin();
        }
        #endregion

        #region Animations
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

        private void ExpanderContentAnimationCollapsed(TimeSpan timeSpan)
        {
            if (!expanderContent.TransformInitialize() && isLoaded)
                return;

            var renderTransform = expanderContent.RenderTransform as TranslateTransform;
            double to = expanderHeader.ActualHeight + expanderContent.ActualHeight;
            expanderContent.Visibility = Visibility.Visible;

            #region yAnimation
            DoubleAnimationUsingKeyFrames yAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            yAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = -to, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(yAnimation, renderTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");
            #endregion

            justStoryboard.Children.Add(yAnimation);
        }
        #endregion
    }
}
