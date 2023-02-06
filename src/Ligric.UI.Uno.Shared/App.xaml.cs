﻿namespace Ligric.UI.Uno
{
    public sealed partial class App : Application
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        private Window? _window;
        public Window? Window => _window;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        public App()
        {
            this.InitializeComponent();
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {

#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
            _window = new Window();
#else
            _window = Microsoft.UI.Xaml.Window.Current;
#endif
            var appRoot = new Shell();
            appRoot.SplashScreen.Initialize(_window, args);

            _window.Content = appRoot;
            _window.Activate();

            _host = await _window.InitializeNavigationAsync(
                    async () =>
                    {
                        return BuildAppHost();
                    },
                    navigationRoot: appRoot.SplashScreen
                );

            var notif = _host.Services.GetRequiredService<IRouteNotifier>();
            notif.RouteChanged += RouteUpdated;

//            var rootFrame = _window.Content as Frame;

//            if (rootFrame == null)
//            {
//                rootFrame = new Frame();
//                _window.Content = rootFrame;
//            }

//#if !(NET6_0_OR_GREATER && WINDOWS)
//            if (args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
//#endif
//            {
//                if (rootFrame.Content == null)
//                {
//                    //var vm = _serviceProvider.GetService<ShellViewModel>();
//                    //var viewtest = _serviceProvider.GetRequiredService<IViewLocator>();

//                    //var view = viewtest.ResolveView(vm);

//                    //rootFrame.Content = view;


//                    GrpcChannel grpcChannel = GrpcChannelHalper.GetGrpcChannel();
//                    var metadataRepository = new MetadataRepository();

//                    var authorizationService = new AuthorizationService(grpcChannel, metadataRepository);

//                    rootFrame.Navigate(typeof(AuthorizationPage));
//                    rootFrame.DataContext = new AuthorizationViewModel(authorizationService);
//                }
//                _window.Activate();
//            }
        }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public void RouteUpdated(object? sender, RouteChangedEventArgs e)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            try
            {
                var rootRegion = e.Region.Root();
                var route = rootRegion.GetRoute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
