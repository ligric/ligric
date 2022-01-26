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
                new PropertyMetadata(SnakeExpandDirection.CollapsedFromTopToBottom/*SnakeExpandDirection.CollapsedFromTopToBottom*/, propertyChangedCallback: OnExpandDirectionChanged));

        private static void OnExpandDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (SnakeMenu)d;
            if (thisObject == null)
                return;

            thisObject.UpdateSnakeExpandDirectionState(true);
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


            UpdateSnakeExpandDirectionState(false);
            //_eventSubscriptions.Disposable = disposable;
        }

        private void UpdateSnakeExpandDirectionState(bool useTransitions)
        {
            var snakeExpandDirection = ExpandDirection;

            switch(snakeExpandDirection)
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
