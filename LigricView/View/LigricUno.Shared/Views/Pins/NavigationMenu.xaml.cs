using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricUno.Shared.Views.Pins
{
    public sealed partial class NavigationMenu : UserControl
    {
        public NavigationMenu()
        {
            this.InitializeComponent();

            //////// TEMPRARY 
            menu.ExpanderSideChanged += OnMenuSideChanged;
        }

        //////// TEMPRARY 
        private void OnMenuSideChanged(object sender, LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            if (newSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSidee", false);
            }
            else
            {
                //VisualStateManager.GoToState(menu, "ExpanderSettingsForBottomSidee", false);
            }
        }

        //////// TEMPRARY 
        private void TestButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
        }
    }
}
