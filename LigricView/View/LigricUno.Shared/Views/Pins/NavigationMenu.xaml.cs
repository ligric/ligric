using Windows.UI.Xaml.Controls;

namespace LigricUno.Shared.Views.Pins
{
    public sealed partial class NavigationMenu : UserControl
    {
        public NavigationMenu() => this.InitializeComponent();

        private void TestButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
        }
    }
}
