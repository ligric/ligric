using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricBoardCustomControls.Menus
{
    public partial class SuctionCup : ToggleButton
    {
        public object PopupContent
        {
            get { return GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }
        public static DependencyProperty PopupContentProperty
        {
            get;
        } = DependencyProperty.Register(nameof(PopupContent), typeof(object), typeof(SuctionCup), new PropertyMetadata(null, null));



        public SuctionCup()
        {
            base.DefaultStyleKey = typeof(SuctionCup);
        }
    }
}
