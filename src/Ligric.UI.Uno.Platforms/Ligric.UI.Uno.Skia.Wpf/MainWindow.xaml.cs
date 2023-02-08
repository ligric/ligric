using Window = System.Windows.Window;

namespace Ligric.UI.Uno.WPF
{
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            root.Content = new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new Ligric.UI.Uno.App());
        }
    }
}
