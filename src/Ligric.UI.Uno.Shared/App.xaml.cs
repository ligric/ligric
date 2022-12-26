using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ReactiveUI;
using Splat;
using Windows.ApplicationModel;
using Ligric.UI.ViewModels.Uno;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.Extensions.Logging.EventLog;
using System.Reflection;
using Splat.Microsoft.Extensions.DependencyInjection;
using Ligric.UI.Uno.Pages;

namespace Ligric.UI.Uno
{
    public sealed partial class App
    {
        private Window? _window;

        public IServiceProvider _serviceProvider;

        public App()
        {
            //Initialize();
            InitializeLogging();
            this.InitializeComponent();
#if HAS_UNO || NETFX_CORE
            this.Suspending += OnSuspending;
#endif
        }

        void Initialize()
        {
            var host = Host
              .CreateDefaultBuilder()
              .ConfigureAppConfiguration((hostingContext, config) => {
                  config.Properties.Clear();
                  config.Sources.Clear();
                  hostingContext.Properties.Clear();
              })
              .ConfigureServices(ConfigureServices)
              .Build();

            _serviceProvider = host.Services;
            _serviceProvider.UseMicrosoftDependencyResolver();
        }

        void ConfigureServices(IServiceCollection services)
        {
            services.UseMicrosoftDependencyResolver();
            var resolver = Splat.Locator.CurrentMutable;
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();

            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<IScreen, ShellViewModel>(x => x.GetRequiredService<ShellViewModel>());
            services.AddSingleton<IViewFor<ShellViewModel>, Shell>();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
            _window = new Window();
            _window.Activate();
#else
            _window = Microsoft.UI.Xaml.Window.Current;
#endif

            var rootFrame = _window.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                _window.Content = rootFrame;
            }

#if !(NET6_0_OR_GREATER && WINDOWS)
            if (args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
#endif
            {
                if (rootFrame.Content == null)
                {
                    //var vm = _serviceProvider.GetService<ShellViewModel>();
                    //var viewtest = _serviceProvider.GetRequiredService<IViewLocator>();

                    //var view = viewtest.ResolveView(vm);

                    //rootFrame.Content = view;

                    rootFrame.Navigate(typeof(AuthorizationPage));
                    rootFrame.DataContext = new AuthorizationViewModel();
                }
                _window.Activate();
            }
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

                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);

                builder.AddFilter("Uno", Microsoft.Extensions.Logging.LogLevel.Warning);
                builder.AddFilter("Windows", Microsoft.Extensions.Logging.LogLevel.Warning);
                builder.AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning);
            });

            global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;

#if HAS_UNO
            global::Uno.UI.Adapter.Microsoft.Extensions.Logging.LoggingAdapter.Initialize();
#endif
#endif
        }
    }
}
