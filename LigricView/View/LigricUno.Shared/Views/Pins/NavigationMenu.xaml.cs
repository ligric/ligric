using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricUno.Shared.Views.Pins
{
    public sealed partial class NavigationMenu : UserControl
    {
        public NavigationMenu()
        {
            this.InitializeComponent();

            menu.ExpanderSideChanged += OnMenuSideChanged;
            stackPanel.LayoutUpdated += OnStackPanelLayoutUpdated;
            SetSideSettings(menu.ExpanderSide);
        }

        private void OnStackPanelLayoutUpdated(object sender, object e)
        {
            Rect rect = LayoutInformation.GetLayoutSlot(stackPanel);

            double widthArea = rect.Width - userImage.Margin.Left - userImage.Margin.Right;
            double heightArea = rect.Height - userImage.Margin.Top - userImage.Margin.Bottom;

            if (stackPanel.Orientation == Orientation.Horizontal)
            {
                userImage.Width = heightArea;
            }
            else
            {
                userImage.Height = widthArea;
            }
        }

        private void OnMenuSideChanged(object sender, LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            SetSideSettings(newSide);
        }

        private void SetSideSettings(LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            if (newSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSidee", false);
            }
            else
            {
                VisualStateManager.GoToState(menu, "ExpanderSettingsForBottomSidee", false);
            }
        }

        private void TestButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
        }
    }
}
