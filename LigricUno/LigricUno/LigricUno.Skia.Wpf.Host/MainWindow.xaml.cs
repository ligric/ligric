using System.Windows;

namespace LigricUno.WPF.Host
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            root.Content = new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new LigricUno.App());
        }
    }
}
