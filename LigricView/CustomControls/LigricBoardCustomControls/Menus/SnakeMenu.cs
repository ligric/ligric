using Common;
using LigricMvvmToolkit.Animations;
using System;
using System.Collections.ObjectModel;
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

        protected Border snakeBackgroundElement;
        protected Border expanderContainer;
        protected ContentControl expanderContent;
        protected Border headerConteiner;
        protected Panel headerContent;
        protected ToggleButton toggleButton;

        private bool isApplyed;
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

        protected override void OnApplyTemplate()
        {
            _eventSubscriptions.Disposable = null;
            CompositeDisposable disposables = new CompositeDisposable();

            snakeBackgroundElement = GetTemplateChild(SNAKE_BACKGROUND_ELEMENT) as Border;
            expanderContainer = GetTemplateChild(EXPANDER_CONTEINER) as Border;
            expanderContent = GetTemplateChild(EXPANDER_CONTENT) as ContentControl;

            headerConteiner = GetTemplateChild(HEADER_CONTEINER) as Border;
            headerContent = GetTemplateChild(HEADER_CONTENT) as Panel;
            toggleButton = GetTemplateChild(TOGGLE_BUTTON) as ToggleButton;
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
            syncMethods.WaitingAnotherMethodsAsync(syncMethodIndex++, async () => await SetExpanderSide(expanderSide, false));
            isApplyed = true;
            //SetNewExpanderState(ExpanderState, false);
            //_eventSubscriptions.Disposable = disposable;
        }

        private void SetNewExpanderState(ExpanderState expanderState, ExpanderSide expanderSide, bool useTransitions)
        {
            if (expanderSide == ExpanderSide.Left)
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSide", false);

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
                VisualStateManager.GoToState(this, "ExpanderSettingsForBottomSide", useTransitions);

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

            thisObject.syncMethods.WaitingAnotherMethodsAsync(thisObject.syncMethodIndex++, () => thisObject.SetNewExpanderState(newValue, thisObject.ExpanderSide, true));
        }

        private static void OnExpanderSideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            if (!thisObject.isApplyed)
                return;

            ExpanderSide newValue = (ExpanderSide)e.NewValue;

            thisObject.syncMethods.WaitingAnotherMethodsAsync(thisObject.syncMethodIndex++, async () => await thisObject.SetExpanderSide(newValue, true));
        }

        private async Task SetExpanderSide(ExpanderSide newValue,bool useTransitions)
        {
            if (newValue == ExpanderSide.Left)
            {
                var test = GetTemplateChild("BottomToBufferStoryboard");
                var test2 = GetTemplateChild("BufferToLeftSideStoryboard");
                
                if (GetTemplateChild("BottomToBufferStoryboard") is Storyboard bottomToBufferStoryboard && GetTemplateChild("BufferToLeftSideStoryboard") is Storyboard bufferToLeftSideStoryboard)
                {
                    await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => bottomToBufferStoryboard, () =>
                    {
                        SetNewExpanderState(ExpanderState.Collapsed, newValue, useTransitions);
                        ExpanderSideChanged?.Invoke(this, newValue);
                    });

                    await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => bufferToLeftSideStoryboard, null);

                }
            }
            else if (newValue == ExpanderSide.Bottom)
            {
                var test = GetTemplateChild("LeftToBufferStoryboard");
                var test2 = GetTemplateChild("BufferToBottomSideStoryboard");

                if (GetTemplateChild("LeftToBufferStoryboard") is Storyboard leftToBufferStoryboard && GetTemplateChild("BufferToBottomSideStoryboard") is Storyboard bufferToBottomSideStoryboard)
                {
                    await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => leftToBufferStoryboard, () =>
                    {
                        SetNewExpanderState(ExpanderState.Collapsed, newValue, useTransitions);
                        ExpanderSideChanged?.Invoke(this, newValue);
                    });

                    await syncBufferAnimations.ExecuteAnimationAsync(syncBufferAnimationIndex++, () => bufferToBottomSideStoryboard, null);
                }
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
            //Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(VerticalAlignment.Center));
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
        }
#endif
    }
}
