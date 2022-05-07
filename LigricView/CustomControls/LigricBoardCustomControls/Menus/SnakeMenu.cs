using Common;
using LigricMvvmToolkit.Animations;
using System;
using System.Threading.Tasks;
using Uno.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

namespace LigricBoardCustomControls.Menus
{
    public enum ExpanderSide
    {
        Left,
        Bottom
    }

    public enum ExpanderState
    {
        Expanded,
        Collapsed
    }

    public class ExpanderStateToBoolenConverter :  IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((ExpanderState)value == ExpanderState.Expanded)
                return true;
            else if ((ExpanderState)value == ExpanderState.Collapsed)
                return false;
            else
                throw new NotImplementedException("Stranger ExpanderState value.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return ExpanderState.Expanded;
            else
                return ExpanderState.Collapsed;
        }
    }

    public partial class SnakeMenuTemplateSettings : DependencyObject
    {
        public double HeaderVerticalHeight
        {
            get { return (double)GetValue(HeaderVerticalHeightProperty); }
            set { SetValue(HeaderVerticalHeightProperty, value); }
        }
        public static DependencyProperty HeaderVerticalHeightProperty
        { 
            get; 
        } = DependencyProperty.Register("HeaderVerticalHeight", typeof(double), typeof(SnakeMenuTemplateSettings), new PropertyMetadata(0.0));

        public double HeaderHorizontalHeight
        {
            get { return (double)GetValue(HeaderHorizontalHeightProperty); }
            set { SetValue(HeaderHorizontalHeightProperty, value); }
        }
        public static DependencyProperty HeaderHorizontalHeightProperty
        {
            get;
        } = DependencyProperty.Register("HeaderHorizontalHeight", typeof(double), typeof(SnakeMenuTemplateSettings), new PropertyMetadata(0.0));

        public double HeaderVerticalWidth
        {
            get { return (double)GetValue(HeaderVerticalWidthProperty); }
            set { SetValue(HeaderVerticalWidthProperty, value); }
        }
        public static DependencyProperty HeaderVerticalWidthProperty
        {
            get;
        } = DependencyProperty.Register("HeaderVerticalWidth", typeof(double), typeof(SnakeMenuTemplateSettings), new PropertyMetadata(0.0));

        public double HeaderHorizontalWidth
        {
            get { return (double)GetValue(HeaderHorizontalWidthProperty); }
            set { SetValue(HeaderHorizontalWidthProperty, value); }
        }
        public static DependencyProperty HeaderHorizontalWidthProperty
        {
            get;
        } = DependencyProperty.Register("HeaderHorizontalWidth", typeof(double), typeof(SnakeMenuTemplateSettings), new PropertyMetadata(0.0));
    }


    public delegate void ExpanderStateChangedEventArgs(object sender, ExpanderState newState);
    public delegate void ExpanderSideChangedEventArgs(object sender, ExpanderSide newSide);

    public partial class SnakeMenu : ContentControl
    {
        private const string SNAKE_BACKGROUND_ELEMENT = "SnakeBackgroundElement";
        private const string EXPANDER_CONTEINER = "ExpanderContainer";
        private const string EXPANDER_CONTENT = "ExpanderContent";
        private const string HEADER_CONTEINER = "HeaderConteiner";
        private const string HEADER_CONTENT = "HeaderContent";
        private const string TOGGLE_BUTTON = "ToggleButton";
        private const string ROOT = "Root";

        protected Border snakeBackgroundElement;
        protected Border expanderContainer;
        protected ContentControl expanderContent;
        protected Border headerConteiner;
        protected Panel headerContent;
        protected ToggleButton toggleButton;
        protected FrameworkElement root;

        private bool isApplyed, isChanging, useAnimation = false;
        private SerialDisposable _eventSubscriptions = new SerialDisposable();

        private readonly SyncAnimations syncBufferAnimations = new SyncAnimations();
        private readonly SyncMethod syncMethods = new SyncMethod();
        private int syncBufferAnimationIndex = 0, syncMethodIndex = 0;

        public event ExpanderStateChangedEventArgs ExpanderStateChanged;
        public event ExpanderSideChangedEventArgs ExpanderSideChanged;

        public ExpanderState ExpanderState
        {
            get { return (ExpanderState)GetValue(ExpanderStateProperty); }
            set { SetValue(ExpanderStateProperty, value); }
        }
        public static DependencyProperty ExpanderStateProperty
        {
            get;
        } = DependencyProperty.Register("ExpanderState", typeof(ExpanderState), typeof(SnakeMenu),
                new PropertyMetadata(ExpanderState.Collapsed, propertyChangedCallback: OnExpanderStateChanged));

        public ExpanderSide ExpanderSide
        {
            get { return (ExpanderSide)GetValue(ExpanderSideProperty); }
            set { SetValue(ExpanderSideProperty, value); }
        }
        public static DependencyProperty ExpanderSideProperty
        {
            get;
        } = DependencyProperty.Register("ExpanderSide", typeof(ExpanderSide), typeof(SnakeMenu),
                new PropertyMetadata(defaultValue: ExpanderSide.Bottom, propertyChangedCallback: OnExpanderSideChanged));

        public object Header
        {
            get { return GetValue(HeaderProperty);}
            set { SetValue(HeaderProperty, value); }
        }
        public static DependencyProperty HeaderProperty
        { 
            get; 
        } = DependencyProperty.Register("Header", typeof(object), typeof(SnakeMenu), new PropertyMetadata(null));

        public SnakeMenuTemplateSettings TemplateSettings
        {
            get { return (SnakeMenuTemplateSettings)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
        }
        public static DependencyProperty TemplateSettingsProperty 
        { 
            get; 
        } = DependencyProperty.Register("TemplateSettings", typeof(SnakeMenuTemplateSettings), typeof(SnakeMenu), new PropertyMetadata(null));


        protected override void OnApplyTemplate()
        {
            ElementAddFinished(this, () => 
            {
                _eventSubscriptions.Disposable = null;
                CompositeDisposable disposables = new CompositeDisposable();

                snakeBackgroundElement = GetTemplateChild(SNAKE_BACKGROUND_ELEMENT) as Border;
                expanderContainer = GetTemplateChild(EXPANDER_CONTEINER) as Border;
                expanderContent = GetTemplateChild(EXPANDER_CONTENT) as ContentControl;

                headerConteiner = GetTemplateChild(HEADER_CONTEINER) as Border;
                headerContent = GetTemplateChild(HEADER_CONTENT) as Panel;
                toggleButton = GetTemplateChild(TOGGLE_BUTTON) as ToggleButton;
                root = GetTemplateChild(ROOT) as FrameworkElement;

                ((FrameworkElement)Parent).LayoutUpdated += OnParentLayoutUpdated;

                if (toggleButton != null)
                {
                    //toggleButton.Checked += OnToggleButtonChecked;
                    //toggleButton.Unchecked += OnToggleButtonUnchecked;
                    //disposables.Add(delegate
                    //{
                    //    toggleButton.Checked -= OnToggleButtonChecked;
                    //    toggleButton.Unchecked -= OnToggleButtonUnchecked;
                    //});
                }

                var expanderSide = ExpanderSide;

                syncMethods.WaitingAnotherMethodsAsync(syncMethodIndex++,
                    async () => await SetExpanderSide(expanderSide, false), expanderSide.ToString());

                isApplyed = true;
                useAnimation = true;
            });

            
            //SetNewExpanderState(ExpanderState, false);
            //_eventSubscriptions.Disposable = disposable;
        }

        private void SetNewExpanderState(ExpanderState expanderState, ExpanderSide expanderSide, bool useTransitions)
        {
            if (expanderSide == ExpanderSide.Left)
            {
                root.ClearValue(FrameworkElement.WidthProperty);

                if (expanderState == ExpanderState.Collapsed)
                {
                    VisualStateManager.GoToState(this, "CollapsingFromRightToLeft", useTransitions);
                }
                else
                {
                    VisualStateManager.GoToState(this, "ExpandingFromLeftToRight", useTransitions);
                }
            }

            if (expanderSide == ExpanderSide.Bottom)
            {
                root.ClearValue(FrameworkElement.HeightProperty);

                if (expanderState == ExpanderState.Collapsed)
                {
                    VisualStateManager.GoToState(this, "CollapsingFromTopToBottom", useTransitions);
                }
                else
                {
                    VisualStateManager.GoToState(this, "ExpandingFromBottomToTop", useTransitions);
                }
            }

            ExpanderStateChanged?.Invoke(this, expanderState);
        }

        private static void OnExpanderStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            if (!thisObject.isApplyed)
                return;

            ExpanderState newValue = (ExpanderState)e.NewValue;

            ExpanderState oldValue = (ExpanderState)e.OldValue;

            if (newValue == oldValue)
                return;

            thisObject.syncMethods.WaitingAnotherMethodsAsync(thisObject.syncMethodIndex++, 
                () => thisObject.SetNewExpanderState(newValue, thisObject.ExpanderSide, thisObject.useAnimation));
        }

        private static void OnExpanderSideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            var useAnimation = thisObject.useAnimation;

            if (!thisObject.isApplyed)
                return;

            ExpanderSide newValue = (ExpanderSide)e.NewValue;
            ExpanderSide oldValue = (ExpanderSide)e.OldValue;

            if (newValue == oldValue)
            {
                thisObject.useAnimation = true;
                return;
            }

            thisObject.syncMethods.WaitingAnotherMethodsAsync(thisObject.syncMethodIndex++, 
                async () => await thisObject.SetExpanderSide(newValue, useAnimation), newValue.ToString());

            thisObject.useAnimation = true;
        }

        private async Task SetExpanderSide(ExpanderSide newSide, bool useTransitions)
        {
            if (newSide == ExpanderSide.Left)
            {
                await SetLeftSide(newSide, useTransitions);
            }
            else if (newSide == ExpanderSide.Bottom)
            {
                await SetBottomSide(newSide, useTransitions);
            }
        }


        public void SetSideWithoutAnimation(ExpanderSide newSide)
        {
            useAnimation = false;
            ExpanderSide = newSide;
        }

        private async Task SetLeftSide(ExpanderSide newSide, bool useTransitions)
        {
            if (!useTransitions)
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSide", false);

                if (GetTemplateChild("SetHeightFromHeaderVerticalHeight") is Storyboard setHeightFromHeaderVerticalHeight)
                    setHeightFromHeaderVerticalHeight.Begin();

                SetNewExpanderState(ExpanderState.Collapsed, newSide, useTransitions);
                ExpanderSideChanged?.Invoke(this, newSide);                

                Binding binding = new Binding();
                binding.Path = new PropertyPath(nameof(SnakeMenuTemplateSettings.HeaderVerticalHeight));
                binding.Source = TemplateSettings;
                root.SetBinding(FrameworkElement.HeightProperty, binding);
                return;
            }

            if (GetTemplateChild("BottomToBufferStoryboard") is Storyboard bottomToBufferStoryboard && GetTemplateChild("BufferToLeftSideStoryboard") is Storyboard bufferToLeftSideStoryboard)
            {
                isChanging = true;
                await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => bottomToBufferStoryboard, () =>
                {
                    VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSide", false);
                    SetNewExpanderState(ExpanderState.Collapsed, newSide, useTransitions);
                    ExpanderSideChanged?.Invoke(this, newSide);
                    isChanging = false;
                });

                await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => bufferToLeftSideStoryboard, () =>
                {
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath(nameof(SnakeMenuTemplateSettings.HeaderVerticalHeight));
                    binding.Source = TemplateSettings;
                    root.SetBinding(FrameworkElement.HeightProperty, binding);
                });
            }
        }

        private async Task SetBottomSide(ExpanderSide newSide, bool useTransitions)
        {
            if (!useTransitions)
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForBottomSide", useTransitions);

                if (GetTemplateChild("SetWidthFromHeaderHorizontalWidth") is Storyboard setWidthFromHeaderVerticalHeight)
                    setWidthFromHeaderVerticalHeight.Begin();

                SetNewExpanderState(ExpanderState.Collapsed, newSide, useTransitions);
                ExpanderSideChanged?.Invoke(this, newSide);


                Binding binding = new Binding();
                binding.Path = new PropertyPath(nameof(SnakeMenuTemplateSettings.HeaderHorizontalWidth));
                binding.Source = TemplateSettings;
                root.SetBinding(FrameworkElement.WidthProperty, binding);
                return;
            }

            if (GetTemplateChild("LeftToBufferStoryboard") is Storyboard leftToBufferStoryboard && GetTemplateChild("BufferToBottomSideStoryboard") is Storyboard bufferToBottomSideStoryboard)
            {
                isChanging = true;

                await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => leftToBufferStoryboard, () =>
                {
                    VisualStateManager.GoToState(this, "ExpanderSettingsForBottomSide", useTransitions);
                    SetNewExpanderState(ExpanderState.Collapsed, newSide, useTransitions);
                    ExpanderSideChanged?.Invoke(this, newSide);

                    isChanging = false;
                });

                await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => bufferToBottomSideStoryboard, () =>
                {
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath(nameof(SnakeMenuTemplateSettings.HeaderHorizontalWidth));
                    binding.Source = TemplateSettings;
                    root.SetBinding(FrameworkElement.WidthProperty, binding);
                });
            }
        }

        public SnakeMenu()
        {
            base.DefaultStyleKey = typeof(SnakeMenu);

            // Remove the default entrance transition if existed.
            RegisterPropertyChangedCallback(HorizontalAlignmentProperty, (s, e) =>
            {
                throw new NotImplementedException("HorizontalAlignmentProperty action changed doesn't implemented!!");
            });

            SetValue(TemplateSettingsProperty, new SnakeMenuTemplateSettings());

            //Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        private void OnParentLayoutUpdated(object sender, object e)
        {
            if (!isChanging)
            {
                FrameworkElement parent = (FrameworkElement)Parent;
                TemplateSettings.HeaderVerticalHeight = parent.ActualHeight - Margin.Top - Margin.Bottom;
                TemplateSettings.HeaderHorizontalWidth = parent.ActualWidth - Margin.Left - Margin.Right;
            }
        }

        private void ElementAddFinished(FrameworkElement element, Action addFinished)
        {
            EventHandler<object> wrapperLayoutUpdatedstroyboard = null;

            wrapperLayoutUpdatedstroyboard = (s, e) =>
            {
                element.LayoutUpdated -= wrapperLayoutUpdatedstroyboard;
                addFinished?.Invoke();
            };
            element.LayoutUpdated += wrapperLayoutUpdatedstroyboard;
        }

#if NET6_0_ANDROID
        /// <summary>
        /// Its needed for Android. Native constructor, do not use explicitly.
        /// </summary>
        /// <remarks>
        /// Used by the Xamarin Runtime to materialize native objects that may have been
        /// collacted in the manager world.
        /// </remarks>
        public SnakeMenu(IntPtr javaRefenerce, Android.Runtime.JniHandleOwnership transfer)
        {
            base.DefaultStyleKey = typeof(SnakeMenu);
            SetValue(TemplateSettingsProperty, new SnakeMenuTemplateSettings());
        }
#endif
    }
}
