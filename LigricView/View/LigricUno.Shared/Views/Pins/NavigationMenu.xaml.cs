using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricUno.Views.Pins
{
    public sealed partial class NavigationMenu : UserControl
    {
        public NavigationMenu()
        {
            this.InitializeComponent();
            LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            LayoutUpdated -= OnLayoutUpdated;
            menu.ExpanderSideChanged += OnMenuSideChanged;
            ////// TODO : TEMPRARY
            stackPanel.LayoutUpdated += OnStackPanelLayoutUpdated;
            SetSideSettings(menu.ExpanderSide);
        }

        ////// TODO : TEMPRARY
        private void OnStackPanelLayoutUpdated(object sender, object e)
        {
            Rect rect = LayoutInformation.GetLayoutSlot(stackPanel);

            double widthArea = rect.Width - userImage.Margin.Left - userImage.Margin.Right;
            double heightArea = rect.Height - userImage.Margin.Top - userImage.Margin.Bottom;

            double buttonWidthArea = (rect.Width - boards.Margin.Left - boards.Margin.Right) * 1.1;
            double buttonHeightArea = (rect.Height - boards.Margin.Top - boards.Margin.Bottom) * 1.5;

            if (stackPanel.Orientation == Orientation.Horizontal)
            {
                if (userImage.Visibility != Visibility.Collapsed)
                    userImage.Width = heightArea;

                news.Width = buttonHeightArea;
                profile.Width = buttonHeightArea;
                boards.Width = buttonHeightArea;
                settings.Width = buttonHeightArea;
            }
            else
            {
                userImage.Height = widthArea;
                news.Height = buttonWidthArea;
                profile.Height = buttonWidthArea;
                boards.Height = buttonWidthArea;
                settings.Height = buttonWidthArea;
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
                VisualStateManager.GoToState(this, "ExpanderSettingsForBottomSidee", false);
            }
        }

        ////// TODO : TEMPRARY
        private void TestButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (menu.ExpanderSide == LigricBoardCustomControls.Menus.ExpanderSide.Bottom)
            {
                menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
            }
            else if (menu.ExpanderSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            {
                menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Bottom;
            }
        }
    }
}
