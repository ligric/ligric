using Windows.UI.Xaml.Controls;

namespace LigricUno.Views.Pages
{
    public sealed partial class BoardsPage : Page
    {
        public BoardsPage()
        {
            DataContext = new BoardsViewModel();
            this.InitializeComponent();
        }
    }
}
