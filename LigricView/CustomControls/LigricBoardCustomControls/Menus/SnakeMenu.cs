using System;
using System.Collections.ObjectModel;
using Uno.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricBoardCustomControls.Menus
{
    public enum SnakeExpandDirection
    {
        ExpandedFromBottomToTop,
        ExpandedFromLeftToRight,

        CollapsedFromTopToBottom,
        CollapsedFromRightToLeft
    }

    public delegate void ExpanderStateChangedEventArgs(object sender, SnakeExpandDirection newState);

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

        public SnakeExpandDirection ExpandDirection
        {
            get { return (SnakeExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }
        public static DependencyProperty ExpandDirectionProperty
        {
            get;
        } = DependencyProperty.Register("ExpandDirection", typeof(SnakeExpandDirection), typeof(SnakeMenu),
                new PropertyMetadata(SnakeExpandDirection.CollapsedFromTopToBottom, propertyChangedCallback: OnExpandDirectionChanged));


        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        public static DependencyProperty IsExpandedProperty
        {
            get;
        } = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SnakeMenu),
                new PropertyMetadata(defaultValue: false, propertyChangedCallback: OnIsExpandedChanged));

        public SnakeMenu()
        {
            base.DefaultStyleKey = typeof(SnakeMenu);
        }

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

            UpdateSnakeExpandDirectionState(false);
            //_eventSubscriptions.Disposable = disposable;
        }

        //private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        //{
        //    ExpanderStateChanged?.Invoke(this, this.ExpandDirection);
        //}

        //private void OnToggleButtonUnchecked(object sender, RoutedEventArgs e)
        //{
        //    ExpanderStateChanged?.Invoke(this, this.ExpandDirection);
        //}

        private void UpdateSnakeExpandDirectionState(bool useTransitions)
        {
            var snakeExpandDirection = ExpandDirection;
            SetNewSnakeExpandDirectionState(useTransitions, snakeExpandDirection);
        }

        private void SetNewSnakeExpandDirectionState(bool useTransitions, SnakeExpandDirection snakeExpandDirection)
        {
            switch (snakeExpandDirection)
            {
                ////// Set vertical visual state
                case SnakeExpandDirection.CollapsedFromTopToBottom:
                    VisualStateManager.GoToState(this, "CollapsingFromTopToBottom", useTransitions);
                    break;
                ////// Set vertical visual state
                case SnakeExpandDirection.ExpandedFromBottomToTop:
                    VisualStateManager.GoToState(this, "ExpandingFromBottomToTop", useTransitions);
                    break;
                ////// Set horizontal visual state
                case SnakeExpandDirection.CollapsedFromRightToLeft:
                    VisualStateManager.GoToState(this, "CollapsingFromRightToLeft", useTransitions);
                    break;
                ////// Set horizontal visual state
                case SnakeExpandDirection.ExpandedFromLeftToRight:
                    VisualStateManager.GoToState(this, "ExpandingFromLeftToRight", useTransitions);
                    break;
                default:
                    throw new ArgumentException("[404] Uknown state of SnakeExpandDirection");
            };
        }

        private static void OnExpandDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            thisObject.UpdateSnakeExpandDirectionState(true);
        }


        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;

            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            thisObject.IsExpandedUpdated(newValue);
        }

        private void IsExpandedUpdated(bool expanded)
        {
            var stringSnakeExpandDirection = ExpandDirection.ToString();

            if (stringSnakeExpandDirection.Contains("Left") && stringSnakeExpandDirection.Contains("Right"))
            {
                if (expanded)
                {
                    SetNewSnakeExpandDirectionState(true, SnakeExpandDirection.ExpandedFromLeftToRight);
                }
                else
                {
                    SetNewSnakeExpandDirectionState(true, SnakeExpandDirection.CollapsedFromRightToLeft);
                }
                
            }
            else
            {
                if (expanded)
                {
                    SetNewSnakeExpandDirectionState(true, SnakeExpandDirection.ExpandedFromBottomToTop);
                }
                else
                {
                    SetNewSnakeExpandDirectionState(true, SnakeExpandDirection.CollapsedFromTopToBottom);
                }
            }
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

        }
#endif
    }
}
