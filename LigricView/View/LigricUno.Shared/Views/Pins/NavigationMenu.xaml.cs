using Windows.UI.Xaml.Controls;

namespace LigricUno.Shared.Views.Pins
{
    public sealed partial class NavigationMenu : UserControl
    {
        public NavigationMenu()
        {
            this.InitializeComponent();
            menu.ExpanderSideChanged += OnMenuSideChanged;
        }

        private void OnMenuSideChanged(object sender, LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            if (newSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            {
                stackPanel.Orientation = Orientation.Vertical;
            }
            else
            {
                stackPanel.Orientation = Orientation.Horizontal;
            }
        }

        private void TestButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
        }
    }
}
