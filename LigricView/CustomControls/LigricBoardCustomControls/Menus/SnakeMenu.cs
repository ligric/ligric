using LigricMvvmToolkit.Animations;
using System;
using System.Collections.ObjectModel;
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

        private SerialDisposable _eventSubscriptions = new SerialDisposable();

        public ExpanderState ExpanderState
        {
            get { return (ExpanderState)GetValue(ExpanderStateProperty); }
            set { SetValue(ExpanderStateProperty, value); }
        }
        public static DependencyProperty ExpanderStateProperty
        {
            get;
        } = DependencyProperty.Register("ExpanderState", typeof(ExpanderState), typeof(SnakeMenu),
                new PropertyMetadata(ExpanderState.Collapsed, propertyChangedCallback: OnExpandDirectionChanged));


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

        public event ExpanderStateChangedEventArgs ExpanderStateChanged;

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

            UpdateSnakeExpandDirectionState(null, ExpanderState, false);
            //_eventSubscriptions.Disposable = disposable;
        }

        private void UpdateSnakeExpandDirectionState(ExpanderState? oldValue, ExpanderState newValue, bool useTransitions)
        {
            if (oldValue == null || oldValue == newValue)
            {
                SetNewSnakeExpandDirectionState(useTransitions, newValue);
            }
            else
            {
                SetNewSnakeAxesTransition((ExpanderState)oldValue, newValue);
            }
           
        }

        private SyncAnimations syncBufferAnimations = new SyncAnimations();
        private int syncBufferAnimationIndex = 0;
        private void SetNewSnakeAxesTransition(ExpanderState oldSnakeDirectionState, ExpanderState newSnakeDirectionState)
        {
            if (GetTemplateChild("BottomToBufferStoryboard") is Storyboard bottomToBufferStoryboard && GetTemplateChild("BufferToLeftSideStoryboard") is Storyboard bufferToLeftSideStoryboard)
            {
                syncBufferAnimations.ExecuteAnimation(syncBufferAnimationIndex++, () => bottomToBufferStoryboard, null );
                syncBufferAnimations.ExecuteAnimation(syncBufferAnimationIndex++, () => bufferToLeftSideStoryboard, null);
            }


            //BottomToBuffer

            //VisualStateManager.GoToState(this, "BottomToRight", true);
            //switch (snakeExpandDirection)
            //{
            //    ////// Set vertical visual state
            //    case ExpanderState.ExpandedFromBottomToTop:
            //        //VisualStateManager.GoToState(this, "TopToRight", true);
            //        break;

            //    default:
            //        throw new ArgumentException("[404] Uknown state of ExpanderState");
            //};

        }

        private void SetNewSnakeExpandDirectionState(bool useTransitions, ExpanderState snakeExpandDirection)
        {
            //switch (snakeExpandDirection)
            //{
            //    ////// Set vertical visual state
            //    case ExpanderState.CollapsedFromTopToBottom:
            //        VisualStateManager.GoToState(this, "CollapsingFromTopToBottom", useTransitions);
            //        break;
            //    ////// Set vertical visual state
            //    case ExpanderState.ExpandedFromBottomToTop:
            //        VisualStateManager.GoToState(this, "ExpandingFromBottomToTop", useTransitions);
            //        break;
            //    ////// Set horizontal visual state
            //    case ExpanderState.CollapsedFromRightToLeft:
            //        VisualStateManager.GoToState(this, "CollapsingFromRightToLeft", useTransitions);
            //        break;
            //    ////// Set horizontal visual state
            //    case ExpanderState.ExpandedFromLeftToRight:
            //        VisualStateManager.GoToState(this, "ExpandingFromLeftToRight", useTransitions);
            //        break;
            //    default:
            //        throw new ArgumentException("[404] Uknown state of ExpanderState");
            //};
            ExpanderStateChanged?.Invoke(this, snakeExpandDirection);
        }

        private static void OnExpandDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            var oldValue = e.OldValue;
            var newValue = e.NewValue;

            thisObject.UpdateSnakeExpandDirectionState((ExpanderState?)oldValue, (ExpanderState)newValue, true);
        }

        private static void OnExpanderSideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;

            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            thisObject.IsExpandedUpdated(newValue);
        }

        private void IsExpandedUpdated(bool expanded)
        {
           // var stringSnakeExpandDirection = ExpandDirection.ToString();

            //if (stringSnakeExpandDirection.Contains("Left") && stringSnakeExpandDirection.Contains("Right"))
            //{
            //    if (expanded)
            //    {
            //        SetNewSnakeExpandDirectionState(true, ExpanderState.ExpandedFromLeftToRight);
            //    }
            //    else
            //    {
            //        SetNewSnakeExpandDirectionState(true, ExpanderState.CollapsedFromRightToLeft);
            //    }
                
            //}
            //else
            //{
            //    if (expanded)
            //    {
            //        SetNewSnakeExpandDirectionState(true, ExpanderState.ExpandedFromBottomToTop);
            //    }
            //    else
            //    {
            //        SetNewSnakeExpandDirectionState(true, ExpanderState.CollapsedFromTopToBottom);
            //    }
            //}
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
