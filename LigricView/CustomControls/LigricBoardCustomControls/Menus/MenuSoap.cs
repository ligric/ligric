using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LigricBoardCustomControls.Menus
{
    public partial class MenuSoap : Expander
    {
        private bool isLoaded;


        protected readonly string c_expanderHeader = "ExpanderHeader";
        protected readonly string c_expanderContent = "ExpanderContent";


        private FrameworkElement expanderHeader;
        private FrameworkElement expanderContent;

        private enum HeaderSideEnum
        {
            Left,
            Right,
        }

        private HeaderSideEnum headerSide;

        #region MainParentProperty
        public static DependencyProperty MainParentProperty { get; } = DependencyProperty.Register("MainParent", typeof(FrameworkElement), typeof(MenuSoap), new PropertyMetadata(null));
       
        public FrameworkElement MainParent
        {
            get { return (FrameworkElement)GetValue(MainParentProperty); }
            set { SetValue(MainParentProperty, value); }
        }
        #endregion

        #region HeaderBufferProperty
        public FrameworkElement HeaderBuffer
        {
            get { return (FrameworkElement)GetValue(HeaderBufferProperty); }
            set { SetValue(HeaderBufferProperty, value); }
        }

        public static DependencyProperty HeaderBufferProperty { get; } = DependencyProperty.Register("HeaderBuffer", typeof(FrameworkElement), typeof(MenuSoap), new PropertyMetadata(null, OnHeaderBufferChanged));

        private static void OnHeaderBufferChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newBuffer = e.NewValue as FrameworkElement;
            if (newBuffer is null)
                return;

            var thisObject = (MenuSoap)d;

            if (!thisObject.IsLoaded)
                return;

            BufferForm(thisObject, newBuffer);
        }
        #endregion

        private static void BufferForm(MenuSoap thisObject, FrameworkElement buffer)
        {
            ////// Set start Size
            thisObject.expanderHeader.Width = buffer.ActualWidth;
            thisObject.expanderHeader.Height = buffer.ActualHeight;

            ////// Set start Postion
            var elementVisualRelative = buffer.TransformToVisual(thisObject.MainParent);
            Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).X = bufferPostition.X;
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).Y = bufferPostition.Y;


            ////// Size action changing
            buffer.SizeChanged += (sender, eventArgs) =>
            {
                thisObject.expanderHeader.Width = eventArgs.NewSize.Width;
                thisObject.expanderHeader.Height = eventArgs.NewSize.Height;
            };

            ////// Position action changing

            // TODO : тут должна быть привязка к изменении позици buffer'а
        }

        public MenuSoap() : base()
        {
            this.DefaultStyleKey = typeof(MenuSoap);

            this.Loaded += OnMenuSoapLoaded;

            this.Expanding += OnMenuStandardExpanding;
            this.Collapsed += OnMenuStandardCollapsed;
        }

        private void OnMenuStandardCollapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderCollapsing();
        }

        private void ExpanderCollapsing()
        {
            expanderContent.Visibility = Visibility.Collapsed;
        }

        private void OnMenuStandardExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderExpanding();
        }

        private void ExpanderExpanding()
        {
            ((TranslateTransform)expanderContent.RenderTransform).X = MainParent.ActualWidth - this.Padding.Left - this.Padding.Right - expanderContent.ActualWidth + 40;

            expanderContent.Visibility = Visibility.Visible;

        }


        #region Initialization
        private void InitializeState()
        {
            BufferForm(this, HeaderBuffer);

            if (this.IsExpanded)
            {
                ExpanderExpanding();
            }
            else
            {
                ExpanderCollapsing();
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

        private void OnMenuSoapLoaded(object sender, RoutedEventArgs e)
        {
            expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;
            expanderContent = GetTemplateChild(c_expanderContent) as FrameworkElement;


            TransformInitialize(expanderHeader);
            TransformInitialize(expanderContent);

            isLoaded = true;
            InitializeState();
        }
    }
}
