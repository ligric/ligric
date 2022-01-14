using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.ToggleButtons
{
    public partial class ToggleButtonStandard : ToggleButton
    {
        private Storyboard justStoryboard = new Storyboard();
        private bool isLoaded;

        protected readonly string c_elipse = "Elipse";

        private FrameworkElement elipse;
        
        public ToggleButtonStandard() : base() 
        {
            this.DefaultStyleKey = typeof(ToggleButtonStandard);

            this.Loaded += OnToggleButtonLoaded;
        }

        #region Initialization
        private void InitializeState()
        {
            isLoaded = true;

            var elipsePparent = (FrameworkElement)elipse.Parent;
            var elipseHeight = elipsePparent.ActualHeight - 6 > 0 ? elipsePparent.ActualHeight - 6 : 0;

            elipse.Width = elipseHeight;
            elipse.Height = elipseHeight;

            SizeChanged += (s,e) =>
            {
                var newElipsePparent = (FrameworkElement)elipse.Parent;
                var newElipseHeight = newElipsePparent.ActualHeight - 6 > 0 ? newElipsePparent.ActualHeight - 6 : 0;

                elipse.Width = newElipseHeight;
                elipse.Height = newElipseHeight;

                if ((bool)this.IsChecked)
                    CheckedInitialize();
                else
                    UncheckedInitialize();

                CornerRadius = new CornerRadius(this.ActualHeight / 2);
            };

            if ((bool)this.IsChecked)
                CheckedInitialize();
            else
                UncheckedInitialize();
        }


        private void CheckedInitialize()
        {
            var newX = ((FrameworkElement)elipse.Parent).ActualWidth - ((FrameworkElement)elipse.Parent).ActualHeight - (elipse.Parent is Border ? ((Border)elipse.Parent).Padding.Right : 0);
            ((TranslateTransform)elipse.RenderTransform).X = newX;
        }

        private void UncheckedInitialize()
        {
            ((TranslateTransform)elipse.RenderTransform).X = 0;
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

        #region ToggleButton actions
        private void OnToggleButtonLoaded(object sender, RoutedEventArgs e)
        {
            this.Checked += OnToggleButtonChecked;

            this.Unchecked += OnToggleButtonUnChecked;

            elipse = GetTemplateChild(c_elipse) as FrameworkElement;
            
            TransformInitialize(elipse);

            
            InitializeState();
        }

        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
                return;

            ToggleButtonChecked();
        }        
        
        private void OnToggleButtonUnChecked(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
                return;

            ToggleButtonUnChecked();
        }
        #endregion


        private void ToggleButtonChecked()
        {
            justStoryboard.Pause();
            justStoryboard = new Storyboard();
            ElipseAnimationChecking(TimeSpan.FromMilliseconds(150));
            justStoryboard.Begin();
        }        
       
        private void ToggleButtonUnChecked()
        {
            justStoryboard.Pause();
            justStoryboard = new Storyboard();
            ElipseAnimationUnChecking(TimeSpan.FromMilliseconds(150));
            justStoryboard.Begin();
        }

        private void ElipseAnimationChecking(TimeSpan timeSpan)
        {
            if (!TransformInitialize(elipse) && isLoaded)
                return;

            var renderTransform = elipse.RenderTransform as TranslateTransform;

            var newPosition = ((FrameworkElement)elipse.Parent).ActualWidth - ((FrameworkElement)elipse.Parent).ActualHeight - (elipse.Parent is Border ? ((Border)elipse.Parent).Padding.Right : 0);

            #region xAnimation
            DoubleAnimationUsingKeyFrames xAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            xAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = newPosition, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion
            justStoryboard.Children.Add(xAnimation);
        }

        private void ElipseAnimationUnChecking(TimeSpan timeSpan)
        {
            if (!TransformInitialize(elipse) && isLoaded)
                return;

            var renderTransform = elipse.RenderTransform as TranslateTransform;

            #region xAnimation
            DoubleAnimationUsingKeyFrames xAnimation = new DoubleAnimationUsingKeyFrames() { EnableDependentAnimation = true };
            xAnimation.KeyFrames.Add(new LinearDoubleKeyFrame() { Value = 0, KeyTime = KeyTime.FromTimeSpan(timeSpan) });
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");
            #endregion
            justStoryboard.Children.Add(xAnimation);
        }
    }
}
