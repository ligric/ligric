using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricBoardCustomControls.Menus
{
    public partial class SuctionCup : ToggleButton
    {
        private const string POPUP = "Popup";
        private const string POPUP_CONTENT = "PopupContent";

        protected Popup _popup;
        protected FrameworkElement _popupContent;


        public object PopupContent
        {
            get { return GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }
        public static DependencyProperty PopupContentProperty
        {
            get;
        } = DependencyProperty.Register(nameof(PopupContent), typeof(object), typeof(SuctionCup), new PropertyMetadata(null, null));


        protected override void OnApplyTemplate()
        {
            _popup = GetTemplateChild(POPUP) as Popup;
            _popupContent = GetTemplateChild(POPUP_CONTENT) as FrameworkElement;
            UpdatePopupMargin();
            _popup.LayoutUpdated += OnPopupContentLayoutUpdated;
        }

        private void OnPopupContentLayoutUpdated(object sender, object e)
        {
            UpdatePopupMargin();
        }

        private void UpdatePopupMargin()
        {
            var contentHeight = _popupContent.ActualHeight;
            _popup.Margin = new Thickness(0, -contentHeight, 0, 0);
        }

        public SuctionCup()
        {
            base.DefaultStyleKey = typeof(SuctionCup);
        }
    }
}
