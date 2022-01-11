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
            this.SizeChanged += OnToggleButtonStandardSizeChanged;

            this.Loaded += OnToggleButtonLoaded;

            this.Checked += OnToggleButtonChecked;

            this.Unchecked += OnToggleButtonUnChecked;
        }

        private void OnToggleButtonStandardSizeChanged(object sender, SizeChangedEventArgs args)
        {
            this.CornerRadius = new CornerRadius((double)this.ActualHeight / 2);
        }


        #region Initialization
        private void InitializeState()
        {
            isLoaded = true;

            var parent = (FrameworkElement)elipse.Parent;
            elipse.Width = parent.ActualHeight - 6;
            elipse.Height = parent.ActualHeight - 6;

            parent.SizeChanged += (s,e) =>
            {
                elipse.Width = parent.ActualHeight - 6;
                elipse.Height = parent.ActualHeight - 6;
            };

            if ((bool)this.IsChecked)
            {
                ToggleButtonChecked();
            }
            else
            {
                ToggleButtonUnChecked();
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

        #region ToggleButton actions
        private void OnToggleButtonLoaded(object sender, RoutedEventArgs e)
        {
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

            var duration = new Duration(timeSpan);

            var renderTransform = elipse.RenderTransform as TranslateTransform;

            DoubleAnimation xAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = 0,
                To = (elipse.Parent is FrameworkElement ? ((FrameworkElement)elipse.Parent).ActualWidth : this.ActualWidth ) - (elipse.Parent is FrameworkElement ? ((FrameworkElement)elipse.Parent).ActualHeight : elipse.ActualHeight) - (elipse.Parent is Border ? ((Border)elipse.Parent).Padding.Right : 0),
                Duration = duration
            };
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");

            justStoryboard.Children.Add(xAnimation);
        }

        private void ElipseAnimationUnChecking(TimeSpan timeSpan)
        {
            if (!TransformInitialize(elipse) && isLoaded)
                return;

            var duration = new Duration(timeSpan);

            var renderTransform = elipse.RenderTransform as TranslateTransform;

            DoubleAnimation xAnimation = new DoubleAnimation()
            {
                EnableDependentAnimation = true,
                From = (elipse.Parent is FrameworkElement ? ((FrameworkElement)elipse.Parent).ActualWidth : this.ActualWidth) - (elipse.Parent is FrameworkElement ? ((FrameworkElement)elipse.Parent).ActualHeight : elipse.ActualHeight) - (elipse.Parent is Border ? ((Border)elipse.Parent).Padding.Right : 0),
                To = 0,
                Duration = duration
            };
            Storyboard.SetTarget(xAnimation, renderTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");

            justStoryboard.Children.Add(xAnimation);
        }
    }
}
