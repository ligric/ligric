using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ReactiveUI;
using Splat;
using Autofac;
using Windows.ApplicationModel;
using Ligric.UI.ViewModels.Uno;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.Extensions.Logging.EventLog;
using System.Reflection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Ligric.UI.Uno
{
    public sealed partial class App
    {
        private Window? _window;

        public IServiceProvider _serviceProvider;

        public App()
        {
            Initialize();
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

            var allTypes = Assembly.GetExecutingAssembly()
              .DefinedTypes
              .Where(t => !t.IsAbstract);

            // register services
            {
                //services.AddSingleton<BookService>();
                //services.AddTransient(typeof(apiClient), x => new apiClient(new HttpClient()));
            }

            // register view models
            {
                services.AddSingleton<ShellViewModel>();
                services.AddSingleton<IScreen>(sp => sp.GetRequiredService<ShellViewModel>());

                var rvms = allTypes.Where(t => typeof(IRoutableViewModel).IsAssignableFrom(t));
                foreach (var rvm in rvms)
                    services.AddTransient(rvm);
            }

            // register views
            {
                var vf = typeof(IViewFor<>);
                bool isGenericIViewFor(Type ii) => ii.IsGenericType && ii.GetGenericTypeDefinition() == vf;
                var views = allTypes
                  .Where(t => t.ImplementedInterfaces.Any(isGenericIViewFor));

                foreach (var v in views)
                {
                    var ii = v.ImplementedInterfaces.Single(isGenericIViewFor);

                    services.AddTransient(ii, v);
                }
            }

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
                    rootFrame.Navigate(typeof(Shell), args.Arguments);

                    //var vm = _serviceProvider.GetService<ShellViewModel>();
                    //var viewtest = _serviceProvider.GetRequiredService<IViewLocator>();

                    //var view = viewtest.ResolveView(vm);

                    //rootFrame.Content = new Shell();
                    //rootFrame.DataContext = view?.ViewModel;
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
