using Ligric.UI.ViewModels.Uno;
using Microsoft.UI.Xaml.Controls;

namespace Ligric.UI.Uno.Pages
{
    public sealed partial class FuturesPage : Page
    {
        public FuturesPage()
        {
            this.InitializeComponent();
            DataContext = new FuturesViewModel();
        }
    }
}
