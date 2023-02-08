using Microsoft.UI.Xaml.Controls;
using Uno.Toolkit.UI;

namespace Ligric.UI
{
    public sealed partial class Shell : UserControl
    {
        public ExtendedSplashScreen SplashScreen => Splash;

        public Shell()
        {
            this.InitializeComponent();
        }
    }
}
