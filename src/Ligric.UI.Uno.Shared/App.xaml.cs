using Ligric.UI.Uno.Pages;
using Ligric.UI.ViewModels.Uno;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Prism.Ioc;
using Prism.Mvvm;
using Windows.ApplicationModel;

namespace Ligric.UI.Uno
{
    public sealed partial class App
    {
        public App()
        {
            InitializeLogging();
            this.InitializeComponent();
#if HAS_UNO || NETFX_CORE
            this.Suspending += OnSuspending;
#endif
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            ConfigureViewSize();
        }

        protected override UIElement CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register(typeof(Shell).ToString(), typeof(ShellViewModel));
            ViewModelLocationProvider.Register(typeof(LoginPage).ToString(), typeof(AuthorizationViewModel));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginPage>(Infrastructure.NavigationViews.AUTHORIZATION);
            containerRegistry.RegisterForNavigation<FuturesPage>(Infrastructure.NavigationViews.FUTURES);
        }

        private void ConfigureViewSize()
        {
//#if WINDOWS
//			ApplicationView.PreferredLaunchViewSize = new Size(1330, 768);
//			ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
//			ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 480));
//#endif
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private static void InitializeLogging()
        {
#if DEBUG
            var factory = LoggerFactory.Create(builder =>
            {
#if __WASM__
                builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__
                builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
#elif NETFX_CORE
                builder.AddDebug();
#else
                builder.AddConsole();
#endif

                builder.SetMinimumLevel(LogLevel.Information);

                builder.AddFilter("Uno", LogLevel.Warning);
                builder.AddFilter("Windows", LogLevel.Warning);
                builder.AddFilter("Microsoft", LogLevel.Warning);
            });

            global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
            global::Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif
#endif
        }
    }
}
