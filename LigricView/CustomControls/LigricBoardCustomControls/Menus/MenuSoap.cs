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
        public static DependencyProperty MainParentProperty { get; } = DependencyProperty.Register("MainParent", typeof(FrameworkElement), typeof(MenuSoap), new PropertyMetadata(null, OnMainParentChanged));
       
        public FrameworkElement MainParent
        {
            get { return (FrameworkElement)GetValue(MainParentProperty); }
            set { SetValue(MainParentProperty, value); }
        }

        private static void OnMainParentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mainParent = e.NewValue as FrameworkElement;
            if (mainParent is null)
                return;

            var thisObject = (MenuSoap)d;

            BufferUpdate(thisObject);

            mainParent.SizeChanged += (sender, eventArgs) => BufferUpdate(thisObject);
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

            //BufferForm(thisObject, newBuffer);
            //newBuffer.SizeChanged += (sender, eventArgs) => BufferForm(thisObject, newBuffer);
        }
        #endregion

        private static void BufferUpdate(MenuSoap thisObject)
        {
            if (!thisObject.IsLoaded)
                return;

            ////// Set start Size
            thisObject.expanderHeader.Width = thisObject.HeaderBuffer.ActualWidth;
            thisObject.expanderHeader.Height = thisObject.HeaderBuffer.ActualHeight;

            ////// Set start Postion
            var elementVisualRelative = thisObject.HeaderBuffer.TransformToVisual(thisObject.MainParent);
            Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).X = bufferPostition.X;
            ((TranslateTransform)thisObject.expanderHeader.RenderTransform).Y = bufferPostition.Y;
        }

        public MenuSoap() : base()
        {
            this.DefaultStyleKey = typeof(MenuSoap);

            this.Loaded += OnMenuSoapLoaded;
        }

        private void OnMenuSoapExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (!isLoaded)
                return;

            ExpanderInitializeExpanding();
        }

        private void ExpanderInitializeExpanding()
        {
            var elementVisualRelative = HeaderBuffer.TransformToVisual(MainParent);
            Point bufferPostition = elementVisualRelative.TransformPoint(new Point(0, 0));
            ((TranslateTransform)expanderContent.RenderTransform).X = bufferPostition.X - HeaderBuffer.ActualWidth;
        }


        #region Initialization
        private void InitializeState()
        {
            BufferUpdate(this);

            isLoaded = true;

            if (this.IsExpanded)
                ExpanderInitializeExpanding();
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
            this.Expanding += OnMenuSoapExpanding;

            expanderHeader = GetTemplateChild(c_expanderHeader) as FrameworkElement;
            expanderContent = GetTemplateChild(c_expanderContent) as FrameworkElement;


            TransformInitialize(expanderHeader);
            TransformInitialize(expanderContent);

            InitializeState();
        }
    }
}
