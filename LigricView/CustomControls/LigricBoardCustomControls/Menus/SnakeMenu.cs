using System.Collections.ObjectModel;
using Uno.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricBoardCustomControls.Menus
{
    public enum SnakeExpandDirection
    {
        ExpandFromBottomToTop,
        ExpandFromLeftToRight,

        CollapseFromTopToBottom,
        CollapseFromRightToLeft
    }

    public partial class SnakeMenu : ContentControl
    {
        private const string SNAKE_BACKGROUND_ELEMENT = "SnakeBackgroundElement";
        private const string EXPANDER_CONTENT = "ExpanderContent";
        private const string HEADER_CONTEINER = "HeaderConteiner";
        private const string HEADER_CONTENT = "HeaderContent";
        private const string TOGGLE_BUTTON = "ToggleButton";

        protected ToggleButton toggleButton;
        protected ContentControl headerContent;
        protected Border headerConteiner;
        protected ContentControl expanderContent;
        protected Border snakeBackgroundElement;



        private SerialDisposable _eventSubscriptions = new SerialDisposable();

        public SnakeExpandDirection ExpandDirection
        {
            get { return (SnakeExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }
        public static DependencyProperty ExpandDirectionProperty
        {
            get;
        } = DependencyProperty.Register("ExpandDirection", typeof(SnakeExpandDirection), typeof(SnakeMenu), new PropertyMetadata(null));


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

            toggleButton = GetTemplateChild(TOGGLE_BUTTON) as ToggleButton;
            headerContent = GetTemplateChild(HEADER_CONTENT) as ContentControl;
            headerConteiner = GetTemplateChild(HEADER_CONTEINER) as Border;
            expanderContent = GetTemplateChild(EXPANDER_CONTENT) as ContentControl;
            snakeBackgroundElement = GetTemplateChild(SNAKE_BACKGROUND_ELEMENT) as Border;

        }

    }
}
