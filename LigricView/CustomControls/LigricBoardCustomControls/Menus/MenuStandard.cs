using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace LigricBoardCustomControls.Menus
{
    public partial class MenuStandard : Expander
    {
        public FrameworkElement ParentElement { get => (FrameworkElement)GetValue(ParentElementProperty); set => SetValue(ParentElementProperty, value); }
        public static readonly DependencyProperty ParentElementProperty = DependencyProperty.Register(nameof(ParentElement), typeof(FrameworkElement), typeof(MenuStandard), new PropertyMetadata(null));


        public MenuStandard() : base() { }
    }
}
