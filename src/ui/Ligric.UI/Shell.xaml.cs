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
