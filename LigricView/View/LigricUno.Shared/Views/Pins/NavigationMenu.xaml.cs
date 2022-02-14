using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Shapes;

namespace LigricUno.Shared.Views.Pins
{
    public sealed partial class NavigationMenu : UserControl
    {
        public NavigationMenu()
        {
            this.InitializeComponent();

            //////// TEMPRARY 
            menu.ExpanderSideChanged += OnMenuSideChanged;
            SetSideSettings(menu.ExpanderSide);
        }

        //////// TEMPRARY 
        private void OnMenuSideChanged(object sender, LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            SetSideSettings(newSide);
        }

        private void SetSideSettings(LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            Binding binding = new Binding();
            binding.ElementName = "stackPanel"; // элемент-источник

            if (newSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSidee", false);

                binding.Path = new PropertyPath("ActualWidth");
                userImage.SetBinding(Ellipse.HeightProperty, binding);
            }
            else
            {
                VisualStateManager.GoToState(menu, "ExpanderSettingsForBottomSidee", false);

                binding.Path = new PropertyPath("ActualHeight");
                userImage.SetBinding(Ellipse.WidthProperty, binding);
            }
        }

        //////// TEMPRARY 
        private void TestButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
        }
    }
}
